using Corporate.Cashflow.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corporate.Cashflow.Infraestructure.Data
{
    public class TransactionEntityConfiguration : IEntityTypeConfiguration<AccountBalanceEntity>
    {
        public void Configure(EntityTypeBuilder<AccountBalanceEntity> builder)
        {
            builder.ToTable("AccountBalances")
                .Property<uint>("xmin")
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}
