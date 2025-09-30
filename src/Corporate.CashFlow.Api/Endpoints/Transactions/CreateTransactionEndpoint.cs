using Corporate.Cashflow.Application.UseCases.Transactions.Create;
using Corporate.Cashflow.Domain.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Corporate.CashFlow.Api.Endpoints.Transactions
{
    public static class CreateTransactionEndpoint
    {
        public static IEndpointRouteBuilder MapCreateTransactionEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/transactions/{accountId:guid}", async (
                [FromRoute] Guid accountId,
                [FromBody] CreateTransactionRequest request,
                IMediator _mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateTransactionCommand
                {
                    AccountId = accountId,
                    Amount = request.Amount,
                    CategoryId = request.CategoryId,
                    Date = request.Date,
                    Description = request.Description,
                    TransactionType = request.TransactionType
                };

                var result = await _mediator.Send(command, cancellationToken);

                return Results.Created($"/transactions/{{newTransactionId}}", result.Id);
            })
            .WithName("CreateTransaction")
            .WithTags("Transactions");

            return endpoints;
        }


    }

    public class CreateTransactionRequest
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Guid CategoryId { get; set; }
        public ETransactionType TransactionType { get; set; }
    }
}
