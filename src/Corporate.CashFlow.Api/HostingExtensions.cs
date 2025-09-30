using Corporate.Cashflow.Application;
using Corporate.Cashflow.Application.Interfaces;
using Corporate.Cashflow.Application.Middlewares;
using Corporate.Cashflow.Domain;
using Corporate.Cashflow.Infraestructure;
using Corporate.Cashflow.Infraestructure.Kafka;
using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Corporate.CashFlow.Api
{
    public static class HostingExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.AddServiceDefaults();

            builder.Services.ConfigureEndpoints();

            var jwtSettings = builder.Configuration.GetSection("AuthSettings").Get<AuthSettings>();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });
            builder.Services.AddAuthorization();

            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly));

            builder.Services.AddValidatorsFromAssemblyContaining<IApplicationMarker>();
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IGetIdentifier, GetAuthenticatedUserAccountIdentifier>();

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

                options.SchemaFilter<CustomSchemaFilters>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
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
