using System.ComponentModel.DataAnnotations;

namespace Corporate.Cashflow.Domain.Account
{
    public class AccountBalanceEntity : BaseEntity
    {
        public Guid AccountId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Inflows { get; set; }
        public decimal Outflows { get; set; }
        public decimal Balance { get; set; }
    }
}
