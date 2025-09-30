using Corporate.Cashflow.Application.Interfaces;
using Corporate.Cashflow.Domain.Account;
using Corporate.Cashflow.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Cashflow.Infraestructure.EntityFramework
{
    public class CashflowDbContext : DbContext, ICashflowDbContext
    {
        public CashflowDbContext(DbContextOptions<CashflowDbContext> options)
        : base(options)
        {
        }

        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<AccountBalanceEntity> AccountBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CashflowDbContext).Assembly);

        }
    }
}
