using Corporate.Cashflow.Application.UseCases.Accounts.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Corporate.CashFlow.Api.Endpoints.Accounts
{
    public static class GetByIdEndpoint
    {
        public static RouteGroupBuilder MapGetAccountByIdEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetByIdAsync)
                .WithTags("Accounts")
                .WithName("GetAccountById")
                .WithSummary("Get an store by ID.")
                .WithDescription("Retrieves an store by its ID for the authenticated user.")
                .Produces<GetAccountByIdResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
        private static async Task<IActionResult> GetByIdAsync(
            [FromRoute] Guid id,
            ClaimsPrincipal claims,
            IMediator _mediator,
            CancellationToken cancellationToken)
        {

            var response = await _mediator.Send(new GetAccountByIdQuery(id), cancellationToken);

            return response.ToActionResult();
        }
    }
}
