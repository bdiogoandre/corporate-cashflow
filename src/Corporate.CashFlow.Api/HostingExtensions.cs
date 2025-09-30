using Corporate.Cashflow.Application;
using Corporate.Cashflow.Infraestructure;
using Corporate.Cashflow.Infraestructure.Kafka;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Corporate.CashFlow.Api
{
    public static class HostingExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.AddServiceDefaults();

            builder.Services.ConfigureEndpoints();
            
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly));

            builder.Services.AddInfrastructure(builder.Configuration);


            builder.Services.AddKafka(builder.Configuration["Kafka:BootstrapServers"]!);

            return builder;
        }


        private static IServiceCollection ConfigureEndpoints(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CashFlow Api",
                    Description = "Service that manages Merchants Cash Flows",
                });
            });

            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            return services;
        }
    }
}
