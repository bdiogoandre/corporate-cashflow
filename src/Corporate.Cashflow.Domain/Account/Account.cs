using Corporate.Cashflow.Domain.Enums;

namespace Corporate.Cashflow.Domain.Account
{
    public class Account : BaseEntity
    {
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public ECurrency Currency { get; set; }
    }
}
