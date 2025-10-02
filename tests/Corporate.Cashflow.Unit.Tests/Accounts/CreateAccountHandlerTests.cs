using Corporate.Cashflow.Application.Interfaces;
using Corporate.Cashflow.Application.UseCases.Accounts.Create;
using Corporate.Cashflow.Domain.Enums;
using Corporate.Cashflow.Infraestructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Corporate.Cashflow.Unit.Tests.Accounts;

public class CreateAccountHandlerTests
{
	private static CashflowDbContext CreateInMemoryContext()
	{
		var options = new DbContextOptionsBuilder<CashflowDbContext>()
			.UseInMemoryDatabase(databaseName: $"cashflow-tests-{Guid.NewGuid()}")
			.Options;
		return new CashflowDbContext(options);
	}

	[Fact]
	public async Task Handle_Should_Create_Account_And_Return_Id()
	{
		// Arrange
		var context = CreateInMemoryContext();
		var userId = Guid.NewGuid();
		var getIdentifier = new Mock<IGetIdentifier>();
		getIdentifier.Setup(x => x.GetAuthenticatedUserId()).Returns(userId);

		var handler = new Handler(context, getIdentifier.Object);
		var command = new CreateAccountCommand
		{
			Name = "Main Account",
			Currency = ECurrency.USD
		};

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.False(result.IsError);
		Assert.NotEqual(Guid.Empty, result.Value);

		var saved = await context.Accounts.FindAsync(result.Value);
		Assert.NotNull(saved);
		Assert.Equal("Main Account", saved!.Name);
		Assert.Equal(ECurrency.USD, saved.Currency);
		Assert.Equal(userId, saved.UserId);
	}
}
