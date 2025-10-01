using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Domain.Enums;
using ErrorOr;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Accounts.GetById
{
    public record GetAccountByIdQuery(Guid Id) : IRequest<ErrorOr<GetAccountByIdResponse>>;

    public record GetAccountByIdResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public ECurrency Currency { get; set; }
    }
}
