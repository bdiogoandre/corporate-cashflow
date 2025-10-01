using Corporate.Cashflow.Application.UseCases.Accounts.Create;
using Corporate.Cashflow.Application.UseCases.Accounts.GetById;
using Corporate.Cashflow.Domain.Enums;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
                .Produces<ErrorOr<GetAccountByIdResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }

        private static async Task<IResult> CreateAsync(
            [FromBody] CreateAccountRequest request,
            IMediator _mediator,
            CancellationToken cancellationToken)
        {

            var command = new CreateAccountCommand
            {
                Currency = request.Currency,
                Name = request.Name,
            };

            var response = await _mediator.Send(command, cancellationToken);

            if (response.IsError)
            {
                return response.Errors.ToProblem();
            }

            return Results.CreatedAtRoute("CreateAccount", new { id = response.Value }, response.Value);
        }

        private static async Task<IResult> GetByIdAsync(
            [FromRoute] Guid id,
            IMediator _mediator,
            CancellationToken cancellationToken)
        {

            var response = await _mediator.Send(new GetAccountByIdQuery(id), cancellationToken);
            if (response.IsError)
            {
                return response.Errors.ToProblem();
            }

            return Results.Ok(response.Value);
        }
    }

    public class CreateAccountRequest
    {
        public string? Name { get; set; }
        public ECurrency Currency { get; set; }
    }
}
