using Corporate.Cashflow.Consumer;
using Corporate.Cashflow.Infraestructure.Kafka;

var builder = WebApplication.CreateBuilder(args);

var bootstrapServers = "localhost:9092";
var topic = "cashflow-topic";

await KafkaSetup.EnsureTopicExists(bootstrapServers, topic);

builder.Services.AddHostedService<CashflowConsumerService>();

var app = builder.Build();

await app.RunAsync();

