namespace Corporate.Cashflow.Domain.Transactions
{
    public class TransactionEntity : BaseEntity
    {
        public Guid AccountId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Data { get; set; }
        public long Sequence { get; set; }
    }
}
