namespace Corporate.Cashflow.Domain.Transactions
{
    public class TransactionCreated : BaseEntity
    {
        public Guid AccountId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Data { get; set; }
        public long Sequence { get; set; }
    }
}
