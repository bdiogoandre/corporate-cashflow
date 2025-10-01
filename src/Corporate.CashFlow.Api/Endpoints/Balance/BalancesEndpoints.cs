using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Application.UseCases.Balances.GetAll;
using Corporate.Cashflow.Application.UseCases.Balances.GetByDate;
using Corporate.Cashflow.Application.UseCases.Transactions.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Corporate.CashFlow.Api.Endpoints.Balance
{
    public static class BalancesEndpoints
    {
        public static IEndpointRouteBuilder MapBalancesEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/date/{date}", GetBalancesDateAsync)
                .WithTags("Balance")
                .WithName("GetBalancesDate")
                .WithSummary("Get balances for a specific date")
                .ProducesValidationProblem()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status404NotFound);

            endpoints.MapGet("/", GetAllBalancesPaginatedAsync)
                .WithName("Balances")
                .WithTags("Balances")
                .WithSummary("Get All Balances from account with filter")
                .ProducesValidationProblem()
                .Produces<PagedResult<GetAllTransactionsPaginatedResponse>>()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status500InternalServerError);

            return endpoints;
        }

        private static async Task<IActionResult> GetAllBalancesPaginatedAsync(
            [FromRoute] Guid accountId,
            [FromQuery] GetAllBalancesPaginatedRequest request,
            IMediator _mediator,
            CancellationToken cancellationToken)
        {
            var query = new GetAllBalancesPaginatedQuery
            {
                AccountId = accountId,
                InitialDate = request.InitialDate,
                FinalDate = request.FinalDate,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var response = await _mediator.Send(query, cancellationToken);
            return response.ToActionResult();
        }

        private static async Task<IActionResult> GetBalancesDateAsync(
            [FromRoute] DateOnly date,
            [FromRoute] Guid accountId,
            IMediator _mediator,
            CancellationToken cancellationToken)
        {
            var query = new GetBalanceByDateQuery(accountId, date);
            var response = await _mediator.Send(query, cancellationToken);
            return response.ToActionResult();
        }
    }

    public class GetAllBalancesPaginatedRequest : PaginationFilter
    {
        public DateOnly? InitialDate { get; set; }
        public DateOnly? FinalDate { get; set; }
    }
}
