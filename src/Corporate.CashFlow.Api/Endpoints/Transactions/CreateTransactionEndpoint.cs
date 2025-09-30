using Corporate.Cashflow.Application.UseCases.Transactions.Create;
using Corporate.Cashflow.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Corporate.CashFlow.Api.Endpoints.Transactions
{
    public static class CreateTransactionEndpoint
    {
        public static IEndpointRouteBuilder MapCreateTransactionEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/", CreateAsync)
            .WithName("CreateTransaction")
            .WithTags("Transactions")
            .WithSummary("Create Transaction by account id")
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError);

            return endpoints;
        }

        public static async Task<IActionResult> CreateAsync(
            Guid accountId,
            CreateTransactionRequest request,
            IMediator _mediator,
            CancellationToken cancellationToken)
        {
            var command = new CreateTransactionCommand
            {
                AccountId = accountId,
                Amount = request.Amount,
                Date = request.Date,
                Description = request.Description,
                TransactionType = request.TransactionType
            };
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return result.ToActionResult();
            }

            return result.ToActionResult();
        }
    }

    public class CreateTransactionRequest
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public ETransactionType TransactionType { get; set; }
        public EPaymentMethod PaymentMethod { get; set; }
    }
}
