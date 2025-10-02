using Corporate.Cashflow.Application.UseCases.Balances.Consolidate;
using Corporate.Cashflow.Domain.Accounts;
using Corporate.Cashflow.Domain.Enums;
using Corporate.Cashflow.Infraestructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Cashflow.Unit.Tests.Balances;

public class ConsolidateHandlerTests
{
	private static CashflowDbContext CreateInMemoryContext()
	{
		var options = new DbContextOptionsBuilder<CashflowDbContext>()
			.UseInMemoryDatabase(databaseName: $"cashflow-tests-{Guid.NewGuid()}")
			.Options;
		return new CashflowDbContext(options);
	}

	[Fact]
	public async Task Handle_Should_Create_New_Balance_When_Not_Exists()
	{
		var context = CreateInMemoryContext();
		var handler = new Handler(context);
		var accountId = Guid.NewGuid();
		var transactionId = Guid.NewGuid();

		var command = new ConsolidationCommand
		{
			TransactionId = transactionId,
			AccountId = accountId,
			Date = DateTimeOffset.UtcNow,
			Amount = 100,
			Description = "Initial inflow",
			TransactionType = ETransactionType.Inflow
		};

		await handler.Handle(command, CancellationToken.None);

		var date = DateOnly.FromDateTime(DateTime.UtcNow.Date);
		var balance = await context.AccountBalances.FirstOrDefaultAsync(x => x.AccountId == accountId && x.Date == date);

		Assert.NotNull(balance);
		Assert.Equal(100, balance!.Inflows);
		Assert.Equal(0, balance.Outflows);
		Assert.Equal(100, balance.Balance);
		Assert.Equal(transactionId, balance.LastTransactionId);
	}

	[Fact]
	public async Task Handle_Should_Accumulate_Inflows_And_Outflows()
	{
		var context = CreateInMemoryContext();
		var handler = new Handler(context);
		var accountId = Guid.NewGuid();
		var date = DateTimeOffset.UtcNow;

		await handler.Handle(new ConsolidationCommand
		{
			TransactionId = Guid.NewGuid(),
			AccountId = accountId,
			Date = date,
			Amount = 200,
			Description = "Salary",
			TransactionType = ETransactionType.Inflow
		}, CancellationToken.None);

		await handler.Handle(new ConsolidationCommand
		{
			TransactionId = Guid.NewGuid(),
			AccountId = accountId,
			Date = date,
			Amount = 50,
			Description = "Groceries",
			TransactionType = ETransactionType.Outflow
		}, CancellationToken.None);

		var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
		var balance = await context.AccountBalances.FirstAsync(x => x.AccountId == accountId && x.Date == today);

		Assert.Equal(200, balance.Inflows);
		Assert.Equal(50, balance.Outflows);
		Assert.Equal(150, balance.Balance);
	}

	[Fact]
	public async Task Handle_Should_Be_Idempotent_For_Same_Transaction()
	{
		var context = CreateInMemoryContext();
		var handler = new Handler(context);
		var accountId = Guid.NewGuid();
		var txId = Guid.NewGuid();
		var date = DateTimeOffset.UtcNow;

		var cmd = new ConsolidationCommand
		{
			TransactionId = txId,
			AccountId = accountId,
			Date = date,
			Amount = 75,
			Description = "Refund",
			TransactionType = ETransactionType.Inflow
		};

		await handler.Handle(cmd, CancellationToken.None);
		await handler.Handle(cmd, CancellationToken.None);

		var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
		var balance = await context.AccountBalances.FirstAsync(x => x.AccountId == accountId && x.Date == today);

		Assert.Equal(75, balance.Inflows);
		Assert.Equal(75, balance.Balance);
	}
}
