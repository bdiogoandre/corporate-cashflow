using Corporate.Cashflow.Domain.Account;
using Corporate.Cashflow.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Cashflow.Application.Interfaces
{
    public interface ICashflowDbContext
    {
        DbSet<TransactionEntity> Transactions { get; set; }
        DbSet<AccountBalanceEntity> AccountBalances { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
