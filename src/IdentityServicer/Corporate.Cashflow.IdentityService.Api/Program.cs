using Corporate.Cashflow.IdentityServer.Api;
using Corporate.Cashflow.IdentityServer.Api.Endpoints;
using Corporate.CashFlow.IdentityServer.Api.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<IdentityServerDbContext>();
    await context.Database.MigrateAsync();
}

app.MapLoginEndpoint();
app.MapRegisterEndpoint();
app.MapResetPasswordEndpoint();

await app.RunAsync();