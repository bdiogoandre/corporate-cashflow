namespace Corporate.Cashflow.Domain.Transactions
{
    public class TransactionCreatedData
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public ETransactionType TransactionType { get; set; }
    }
}
