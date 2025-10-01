using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Domain.Enums;
using ErrorOr;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Transactions.GetById
{
    public class GetTransactionByIdQuery : IRequest<ErrorOr<GetTransactionByIdResponse>>
    {
        public Guid Id { get; set; }
    }

    public class GetTransactionByIdResponse
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset Date { get; set; }
        public ETransactionType TransactionType { get; set; }
    }
}
