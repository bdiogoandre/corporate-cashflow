using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Domain.Enums;
using ErrorOr;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Accounts.Create
{
    public class CreateAccountCommand : IRequest<ErrorOr<Guid>>
    {
        public string? Name { get; set; }
        public ECurrency Currency { get; set; }
    }
}
