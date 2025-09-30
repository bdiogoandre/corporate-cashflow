using Corporate.Cashflow.Domain.Account;
using Corporate.Cashflow.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Cashflow.Application.Interfaces
{
    public interface ICashflowDbContext
    {
        DbSet<Transaction> Transactions { get; set; }
        DbSet<AccountBalance> AccountBalances { get; set; }
        DbSet<Account> Accounts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
