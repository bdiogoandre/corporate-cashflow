using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Corporate.CashFlow.IdentityServer.Api.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("IdentityServerSql");
        services.AddDbContext<IdentityServerDbContext>(options => options.UseNpgsql(cs));

        return services;
    }
}