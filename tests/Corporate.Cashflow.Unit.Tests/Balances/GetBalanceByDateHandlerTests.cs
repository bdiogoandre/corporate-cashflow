using Corporate.Cashflow.Application.UseCases.Balances.GetByDate;
using Corporate.Cashflow.Domain.Accounts;
using Corporate.Cashflow.Infraestructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Cashflow.Unit.Tests.Balances;

public class GetBalanceByDateHandlerTests
{
	private static CashflowDbContext CreateInMemoryContext()
	{
		var options = new DbContextOptionsBuilder<CashflowDbContext>()
			.UseInMemoryDatabase(databaseName: $"cashflow-tests-{Guid.NewGuid()}")
			.Options;
		return new CashflowDbContext(options);
	}

	[Fact]
	public async Task Handle_Should_Return_NotFound_When_Missing()
	{
		var context = CreateInMemoryContext();
		var handler = new Handler(context);
		var query = new GetBalanceByDateQuery(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.UtcNow));

		var result = await handler.Handle(query, CancellationToken.None);
		Assert.True(result.IsError);
		Assert.Contains(result.Errors, e => e.Type == ErrorOr.ErrorType.NotFound);
	}

	[Fact]
	public async Task Handle_Should_Return_Balance_When_Found()
	{
		var context = CreateInMemoryContext();
		var accountId = Guid.NewGuid();
		var date = DateOnly.FromDateTime(DateTime.UtcNow);

		context.AccountBalances.Add(new AccountBalance
		{
			AccountId = accountId,
			Date = date,
			Inflows = 300,
			Outflows = 50,
			Balance = 250
		});
		await context.SaveChangesAsync(CancellationToken.None);

		var handler = new Handler(context);
		var query = new GetBalanceByDateQuery(accountId, date);

		var result = await handler.Handle(query, CancellationToken.None);

		Assert.False(result.IsError);
		Assert.Equal(accountId, result.Value.AccountId);
		Assert.Equal(date, result.Value.Date);
		Assert.Equal(300, result.Value.Inflows);
		Assert.Equal(50, result.Value.Outflows);
		Assert.Equal(250, result.Value.Balance);
	}
}
