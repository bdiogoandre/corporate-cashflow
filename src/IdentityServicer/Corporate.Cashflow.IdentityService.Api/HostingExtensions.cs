using Corporate.CashFlow.IdentityServer.Api.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Corporate.Cashflow.IdentityServer.Api;

public static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
        var jwtSettings = builder.Configuration.GetSection("AuthSettings").Get<AuthSettings>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Identity Server Api"
            });
        });

        builder.Services.AddDatabase(builder.Configuration);

        var cs = builder.Configuration.GetConnectionString("IdentityServerSql");
        builder.Services.AddDbContext<IdentityServerDbContext>(opt => opt.UseNpgsql(cs));        
        builder.Services.AddIdentityCore<IdentityUser<Guid>>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<IdentityServerDbContext>();

        
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "JwtBearer";
            options.DefaultChallengeScheme = "JwtBearer";
        })
        .AddJwtBearer("JwtBearer", options =>
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

        return builder;
    }
}