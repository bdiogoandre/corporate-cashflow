using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Application.UseCases.Transactions.Create;
using Corporate.Cashflow.Application.UseCases.Transactions.GetAll;
using Corporate.Cashflow.Application.UseCases.Transactions.GetById;
using Corporate.Cashflow.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Corporate.CashFlow.Api.Endpoints.Transactions
{
    public static class TransactionsEndpoints
    {
        public static IEndpointRouteBuilder MapTransactionsEndpoints(this IEndpointRouteBuilder endpoints)
        {
            /// Create transaction
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

            /// Get transaction by id
            endpoints.MapGet("/{transactionId:guid}", GetAsync)
                .WithName("GetTransaction")
                .WithTags("Transactions")
                .WithSummary("Create Transaction by account id")
                .ProducesValidationProblem()
                .Produces<GetTransactionByIdResponse>()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status500InternalServerError);


            /// Get all transactions by account id with pagination and filters
            endpoints.MapGet("/", GetAllAsync)
                .WithName("GetAllTransactions")
                .WithTags("Transactions")
                .WithSummary("Create Transaction by account id")
                .ProducesValidationProblem()
                .Produces<PagedResult<GetAllTransactionsPaginatedResponse>>()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status500InternalServerError);

            return endpoints;
        }

        public static async Task<IResult> CreateAsync(
            [FromRoute] Guid accountId,
            [FromBody] CreateTransactionRequest request,
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
            var response = await _mediator.Send(command, cancellationToken);

            if (response.IsError)
            {
                return response.Errors.ToProblem();
            }

            return Results.CreatedAtRoute("CreateTransaction", new { id = response.Value }, response.Value);
        }

        public static async Task<IResult> GetAsync(
            [FromRoute] Guid accountId,
            [FromRoute] Guid transactionId,
            IMediator _mediator,
            CancellationToken cancellationToken)
        {
            var query = new GetTransactionByIdQuery
            {
                Id = transactionId
            };
            
            var response = await _mediator.Send(query, cancellationToken);

            if (response.IsError)
            {
                return response.Errors.ToProblem();
            }

            return Results.Ok(response.Value);
        }

        public static async Task<IResult> GetAllAsync(
            [FromRoute] Guid accountId,
            [AsParameters] GetTransactionRequest request,
            IMediator _mediator,
            CancellationToken cancellationToken)
        {
            var query = new GetAllTransactionsPaginatedQuery
            {
                AccountId = accountId,
                InitialDate = request.InitialDate,
                FinalDate = request.FinalDate,
                TransactionType = request.TransactionType,
                PaymentMethod = request.PaymentMethod,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var response = await _mediator.Send(query, cancellationToken);

            if (response.IsError)
            {
                return response.Errors.ToProblem();
            }

            return Results.Ok(response.Value);
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

    public class GetTransactionRequest : PaginationFilter
    {
        public DateTimeOffset? InitialDate { get; set; }
        public DateTimeOffset? FinalDate { get; set; }
        public ETransactionType? TransactionType { get; set; }
        public EPaymentMethod? PaymentMethod { get; set; }
    }
}
