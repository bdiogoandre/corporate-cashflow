using Corporate.Cashflow.Application.UseCases.Balances.GetAll;
using Corporate.Cashflow.Domain.Accounts;
using Corporate.Cashflow.Infraestructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Cashflow.Unit.Tests.Balances;

public class GetAllBalancesHandlerTests
{
	private static CashflowDbContext CreateInMemoryContext()
	{
		var options = new DbContextOptionsBuilder<CashflowDbContext>()
			.UseInMemoryDatabase(databaseName: $"cashflow-tests-{Guid.NewGuid()}")
			.Options;
		return new CashflowDbContext(options);
	}

	private static void SeedBalances(CashflowDbContext context, Guid accountId)
	{
		var start = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-10));
		for (var i = 0; i < 10; i++)
		{
			context.AccountBalances.Add(new AccountBalance
			{
				AccountId = accountId,
				Date = start.AddDays(i),
				Inflows = i * 10,
				Outflows = i,
				Balance = i * 10 - i
			});
		}
		context.SaveChanges();
	}

	[Fact]
	public async Task Handle_Should_Paginate_And_Order_Desc_By_Date()
	{
		var context = CreateInMemoryContext();
		var accountId = Guid.NewGuid();
		SeedBalances(context, accountId);

		var handler = new Handler(context);
		var query = new GetAllBalancesPaginatedQuery
		{
			AccountId = accountId,
			Page = 1,
			PageSize = 3
		};

		var result = await handler.Handle(query, CancellationToken.None);

		Assert.False(result.IsError);
		Assert.Equal(10, result.Value.TotalItems);
		Assert.Equal(3, result.Value.Items.Count);
		Assert.True(result.Value.Items[0].Date > result.Value.Items[1].Date);
	}

	[Fact]
	public async Task Handle_Should_Filter_By_Date_Range()
	{
		var context = CreateInMemoryContext();
		var accountId = Guid.NewGuid();
		SeedBalances(context, accountId);

		var handler = new Handler(context);
		var initial = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-5));
		var final = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-2));
		var query = new GetAllBalancesPaginatedQuery
		{
			AccountId = accountId,
			InitialDate = initial,
			FinalDate = final,
			Page = 1,
			PageSize = 50
		};

		var result = await handler.Handle(query, CancellationToken.None);

		Assert.False(result.IsError);
		Assert.All(result.Value.Items, x => Assert.True(x.Date >= initial && x.Date <= final));
		Assert.Equal(result.Value.TotalItems, result.Value.Items.Count);
	}
}
