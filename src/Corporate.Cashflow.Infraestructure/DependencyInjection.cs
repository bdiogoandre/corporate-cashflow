using Corporate.Cashflow.Application.Interfaces;
using Corporate.Cashflow.Infraestructure.EntityFramework;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Corporate.Cashflow.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ICashflowDbContext, CashflowDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("CashFlowSql")));

            services.AddScoped<ICashflowDbContext, CashflowDbContext>();

            return services;
        }
    }
}
