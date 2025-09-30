using Corporate.Cashflow.Domain.Enums;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Balances.Consolidation
{
    public class ConsolidationCommand : IRequest
    {
        public Guid TransactionId { get; set; }
        public Guid AccountId { get; set; }
        public DateTimeOffset Date { get; set; }
        public required decimal Amount { get; set; }
        public required string Description { get; set; }
        public required ETransactionType TransactionType { get; set; }
    }
}
