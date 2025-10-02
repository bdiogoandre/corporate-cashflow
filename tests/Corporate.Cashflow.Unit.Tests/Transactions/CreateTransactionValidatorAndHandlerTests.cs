using Confluent.Kafka;
using Corporate.Cashflow.Application.UseCases.Transactions.Create;
using Corporate.Cashflow.Domain.Accounts;
using Corporate.Cashflow.Domain.Enums;
using Corporate.Cashflow.Infraestructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Corporate.Cashflow.Unit.Tests.Transactions;

public class CreateTransactionValidatorAndHandlerTests
{
	[Fact]
	public void Validator_Should_Fail_On_Invalid_Inputs()
	{
		var validator = new CreateTransactionCommandValidator();
		var cmd = new CreateTransactionCommand
		{
			Amount = 0,
			Description = "",
			Date = DateTime.UtcNow.AddDays(1),
			AccountId = Guid.Empty,
			TransactionType = (ETransactionType)999,
			PaymentMethod = (EPaymentMethod)999
		};

		var result = validator.Validate(cmd);
		Assert.False(result.IsValid);
		Assert.True(result.Errors.Count >= 5);
	}

	[Fact]
	public void Validator_Should_Pass_On_Valid_Inputs()
	{
		var validator = new CreateTransactionCommandValidator();
		var cmd = new CreateTransactionCommand
		{
			Amount = 10,
			Description = "Coffee",
			Date = DateTime.Now,
			AccountId = Guid.NewGuid(),
			TransactionType = ETransactionType.Inflow,
			PaymentMethod = EPaymentMethod.CreditCard
		};

		var result = validator.Validate(cmd);
		Assert.True(result.IsValid);
	}

	private static CashflowDbContext CreateInMemoryContext()
	{
		var options = new DbContextOptionsBuilder<CashflowDbContext>()
			.UseInMemoryDatabase(databaseName: $"cashflow-tests-{Guid.NewGuid()}")
			.Options;
		return new CashflowDbContext(options);
	}

	[Fact]
	public async Task Handler_Should_Return_NotFound_When_Account_Missing()
	{
		var context = CreateInMemoryContext();
		var producer = new Mock<IProducer<string, string>>();
		var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
		{
			{"Kafka:Topic", "transactions"}
		}).Build();

		var handler = new Handler(producer.Object, context, config);
		var cmd = new CreateTransactionCommand
		{
			Amount = 10,
			Description = "Coffee",
			Date = DateTime.UtcNow,
			AccountId = Guid.NewGuid(),
			TransactionType = ETransactionType.Inflow,
			PaymentMethod = EPaymentMethod.CreditCard
		};

		var result = await handler.Handle(cmd, CancellationToken.None);
		Assert.True(result.IsError);
		Assert.Contains(result.Errors, e => e.Type == ErrorOr.ErrorType.NotFound);
	}

	[Fact]
	public async Task Handler_Should_Create_And_Produce_When_Account_Exists()
	{
		var context = CreateInMemoryContext();
		var account = new Account { Id = Guid.NewGuid(), Name = "Wallet", Currency = ECurrency.USD, UserId = Guid.NewGuid() };
		context.Accounts.Add(account);
		await context.SaveChangesAsync(CancellationToken.None);

		var producer = new Mock<IProducer<string, string>>();
		producer
			.Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new DeliveryResult<string, string>
			{
				Status = PersistenceStatus.Persisted
			});

		var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
		{
			{"Kafka:Topic", "transactions"}
		}).Build();

		var handler = new Handler(producer.Object, context, config);
		var cmd = new CreateTransactionCommand
		{
			Amount = 55.5m,
			Description = "Groceries",
			Date = DateTime.UtcNow,
			AccountId = account.Id,
			TransactionType = ETransactionType.Inflow,
			PaymentMethod = EPaymentMethod.DebitCard
		};

		var result = await handler.Handle(cmd, CancellationToken.None);

		Assert.False(result.IsError);
		Assert.NotEqual(Guid.Empty, result.Value);

		var saved = await context.Transactions.FindAsync(result.Value);
		Assert.NotNull(saved);
		Assert.Equal(account.Id, saved!.AccountId);
		Assert.Equal(55.5m, saved.Amount);

		producer.Verify(p => p.ProduceAsync("transactions", It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()), Times.Once);
	}
}
