namespace Corporate.Cashflow.Domain.Transactions
{
    public class TransactionData
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public ETransactionType TransactionType { get; set; }
    }
}
