using Corporate.Cashflow.Domain.Accounts;
using Corporate.Cashflow.Domain.Enums;

namespace Corporate.Cashflow.Domain.Transactions
{
    public class Transaction : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
        public DateTimeOffset Date { get; set; }
        public required decimal Amount { get; set; }
        public required string Description { get; set; }
        public required ETransactionType TransactionType { get; set; }
        public required EPaymentMethod PaymentMethod { get; set; }
    }
}
