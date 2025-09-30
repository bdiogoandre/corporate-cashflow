using System.ComponentModel.DataAnnotations;

namespace Corporate.Cashflow.Domain.Account
{
    public class AccountBalance : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
        public DateOnly Date { get; set; }
        public decimal Inflows { get; set; }
        public decimal Outflows { get; set; }
        public decimal Balance { get; set; }
        public Guid LastTransactionId { get; set; }

        public void CalculateInflow(decimal amount)
        {
            Inflows += amount;
            Balance += amount;
        }

        public void CalculateOutflow(decimal amount)
        {
            Outflows += amount;
            Balance -= amount;
        }
    }
}
