using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Domain.Enums;
using ErrorOr;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Transactions.Create
{
    public class CreateTransactionCommand : IRequest<ErrorOr<Guid>>
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public Guid AccountId { get; set; }
        public ETransactionType TransactionType { get; set; }
        public EPaymentMethod PaymentMethod { get; set; }
    }

}
