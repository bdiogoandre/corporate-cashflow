using Corporate.Cashflow.Application.UseCases.Accounts.GetById;
using Corporate.Cashflow.Domain.Accounts;
using Corporate.Cashflow.Domain.Enums;
using Corporate.Cashflow.Infraestructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Cashflow.Unit.Tests.Accounts;

public class GetAccountByIdHandlerTests
{
	private static CashflowDbContext CreateInMemoryContext()
	{
		var options = new DbContextOptionsBuilder<CashflowDbContext>()
			.UseInMemoryDatabase(databaseName: $"cashflow-tests-{Guid.NewGuid()}")
			.Options;
		return new CashflowDbContext(options);
	}

	[Fact]
	public async Task Handle_Should_Return_Account_When_Found()
	{
		// Arrange: seed an account
		var context = CreateInMemoryContext();
		var account = new Account
		{
			Id = Guid.NewGuid(),
			Name = "Seeded",
			Currency = ECurrency.USD,
			UserId = Guid.NewGuid()
		};
		context.Accounts.Add(account);
		await context.SaveChangesAsync(CancellationToken.None);

		var handler = new Handler(context);
		var query = new GetAccountByIdQuery(account.Id);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.False(result.IsError);
		Assert.Equal(account.Id, result.Value.Id);
		Assert.Equal("Seeded", result.Value.Name);
		Assert.Equal(ECurrency.USD, result.Value.Currency);
		Assert.Equal(account.UserId, result.Value.UserId);
	}

	[Fact]
	public async Task Handle_Should_Return_NotFound_When_Account_Does_Not_Exist()
	{
		// Arrange: empty context
		var context = CreateInMemoryContext();
		var handler = new Handler(context);
		var query = new GetAccountByIdQuery(Guid.NewGuid());

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		Assert.True(result.IsError);
		Assert.Contains(result.Errors, e => e.Type == ErrorOr.ErrorType.NotFound);
	}
}
