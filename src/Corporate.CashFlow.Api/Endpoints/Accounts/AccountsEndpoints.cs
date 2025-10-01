using Corporate.Cashflow.Application.UseCases.Accounts.Create;
using Corporate.Cashflow.Application.UseCases.Accounts.GetById;
using Corporate.Cashflow.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Corporate.CashFlow.Api.Endpoints.Accounts
{
    public static class AccountsEndpoints
    {
        public static IEndpointRouteBuilder MapAccountsEndpoint(this IEndpointRouteBuilder group)
        {
            group.MapPost("/", CreateAsync)
                .WithTags("Accounts")
                .WithName("CreateAccount")
                .WithSummary("Create a new account")
                .ProducesValidationProblem()
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status409Conflict);

            group.MapGet("/{id:guid}", GetByIdAsync)
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

        private static async Task<IActionResult> CreateAsync(
            [FromBody] CreateAccountRequest request,
            ClaimsPrincipal claims,
            IMediator _mediator,
            CancellationToken cancellationToken)
        {

            var command = new CreateAccountCommand
            {
                Currency = request.Currency,
                Name = request.Name,
            };

            var response = await _mediator.Send(command, cancellationToken);

            return response.ToActionResult();
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

    public class CreateAccountRequest
    {
        public string? Name { get; set; }
        public ECurrency Currency { get; set; }
    }
}
