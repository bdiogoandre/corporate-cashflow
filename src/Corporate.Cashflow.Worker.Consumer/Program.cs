using Corporate.Cashflow.Application;
using Corporate.Cashflow.Domain;
using Corporate.Cashflow.Infraestructure;
using Corporate.Cashflow.Infraestructure.Kafka;
using Corporate.Cashflow.Worker.Consumer;

var builder = WebApplication.CreateBuilder(args);

var bootstrapServers = builder.Configuration["Kafka:BootstrapServers"]!;
var topic = builder.Configuration["Kafka:Topic"]!;

// Usado com o propósito único de 
await KafkaSetup.EnsureTopicExists(bootstrapServers, topic, 2, 3);


builder.Services.AddScoped<IGetIdentifier, GetAapplicationNameIdentifier>();

builder.Services.AddHostedService<CashflowConsumerService>();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly));


builder.Services.AddKafka(bootstrapServers);

var app = builder.Build();

await app.RunAsync();

