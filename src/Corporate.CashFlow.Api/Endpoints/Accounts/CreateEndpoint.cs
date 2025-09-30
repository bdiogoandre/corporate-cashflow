using Corporate.Cashflow.Application.UseCases.Accounts.Create;
using Corporate.Cashflow.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Corporate.CashFlow.Api.Endpoints.Accounts
{
    public static class CreateEndpoint
    {
        public static RouteGroupBuilder MapCreateAccountEndpoint(this RouteGroupBuilder group)
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
    }

    public class CreateAccountRequest
    {
        public string? Name { get; set; }
        public ECurrency Currency { get; set; }
    }
}
