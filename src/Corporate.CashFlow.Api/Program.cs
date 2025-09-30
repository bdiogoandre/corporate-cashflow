using Corporate.Cashflow.Infraestructure.EntityFramework;
using Corporate.CashFlow.Api;
using Corporate.CashFlow.Api.Endpoints.Transactions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapCreateTransactionEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<CashflowDbContext>();
    await context.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();

