using Corporate.Cashflow.Application.Results;
using Corporate.Cashflow.Domain.Enums;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Transactions.Create
{
    public class CreateTransactionCommand : IRequest<Result<Guid>>
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public Guid AccountId { get; set; }
        public ETransactionType TransactionType { get; set; }
    }

}
