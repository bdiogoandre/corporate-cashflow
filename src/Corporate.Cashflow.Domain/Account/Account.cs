namespace Corporate.Cashflow.Domain.Account
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public required string OwnnerName { get; set; }
    }
}
