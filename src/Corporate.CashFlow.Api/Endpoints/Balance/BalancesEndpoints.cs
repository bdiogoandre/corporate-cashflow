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
                .WithTags("Balances")
                .WithName("GetBalancesDate")
                .WithSummary("Get balances for a specific date")
                .ProducesValidationProblem()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status404NotFound);

            endpoints.MapGet("/", GetAllBalancesPaginatedAsync)
                .WithTags("Balances")
                .WithName("GetAllBalances")
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

        private static async Task<IResult> GetAllBalancesPaginatedAsync(
            [FromRoute] Guid accountId,
            [AsParameters] GetAllBalancesPaginatedRequest request,
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
            if (response.IsError)
            {
                return response.Errors.ToProblem();
            }

            return Results.Ok(response.Value);
        }

        private static async Task<IResult> GetBalancesDateAsync(
            [FromRoute] DateOnly date,
            [FromRoute] Guid accountId,
            IMediator _mediator,
            CancellationToken cancellationToken)
        {
            var query = new GetBalanceByDateQuery(accountId, date);
            var response = await _mediator.Send(query, cancellationToken);
            if (response.IsError)
            {
                return response.Errors.ToProblem();
            }

            return Results.Ok(response.Value);
        }
    }
}
