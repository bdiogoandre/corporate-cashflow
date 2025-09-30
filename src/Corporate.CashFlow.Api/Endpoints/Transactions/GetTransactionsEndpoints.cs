using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Application.UseCases.Transactions.GetAll;
using Corporate.Cashflow.Application.UseCases.Transactions.GetById;
using Corporate.Cashflow.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Corporate.CashFlow.Api.Endpoints.Transactions
{
    public static class GetTransactionsEndpoints
    {
        public static IEndpointRouteBuilder MapGetTransactionEndpoints(this IEndpointRouteBuilder endpoints)
        {
            /// Get transaction by id
            endpoints.MapGet("/{transactionId}", GetAsync)
            .WithName("CreateTransaction")
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
            .WithName("CreateTransaction")
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

        public static async Task<IActionResult> GetAsync(
            Guid transactionId,
            IMediator _mediator,
            CancellationToken cancellationToken)
        {
            var query = new GetTransactionByIdQuery
            {
                Id = transactionId
            };
            
            var result = await _mediator.Send(query, cancellationToken);

            return result.ToActionResult();
        }

        public static async Task<IActionResult> GetAllAsync(
            Guid accountId,
            GetTransactionRequest request,
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

            var result = await _mediator.Send(query, cancellationToken);

            return result.ToActionResult();
        }
    }

    public class GetTransactionRequest : PaginationFilter
    {
        public DateTimeOffset? InitialDate { get; set; }
        public DateTimeOffset? FinalDate { get; set; }
        public ETransactionType? TransactionType { get; set; }
        public EPaymentMethod? PaymentMethod { get; set; }
    }
}
