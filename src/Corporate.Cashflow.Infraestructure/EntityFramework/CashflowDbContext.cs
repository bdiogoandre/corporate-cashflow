using Corporate.Cashflow.Application.Interfaces;
using Corporate.Cashflow.Domain.Accounts;
using Corporate.Cashflow.Domain.Transactions;
using Corporate.Cashflow.Infraestructure.EntityFramework.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Cashflow.Infraestructure.EntityFramework
{
    public class CashflowDbContext : DbContext, ICashflowDbContext
    {
        readonly UpdateAuditableEntitiesInterceptor updateAuditableEntitiesInterceptor = new();

        public CashflowDbContext(DbContextOptions<CashflowDbContext> options)
        : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AccountBalance> AccountBalances { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(updateAuditableEntitiesInterceptor);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CashflowDbContext).Assembly);

        }
    }
}
