using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporate.Cashflow.Domain.Account
{
    public class AccountDailyBalanceEntity
    {
        public Guid MerchantId { get; set; }
        public DateOnly Date { get; set; }

        public decimal Inflows { get; set; }
        public decimal Outflows { get; set; }
        public decimal Balance { get; set; }
    }
}
