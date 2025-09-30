using Corporate.Cashflow.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corporate.Cashflow.Infraestructure.Data
{
    public class AccountBalanceEntityConfiguration : IEntityTypeConfiguration<AccountBalance>
    {
        public void Configure(EntityTypeBuilder<AccountBalance> builder)
        {
            builder.ToTable("AccountBalances")
                .Property<uint>("xmin")
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}
