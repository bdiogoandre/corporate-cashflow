using Confluent.Kafka;
using Corporate.Cashflow.Application.UseCases.Balances.Consolidate;
using Corporate.Cashflow.Domain.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using System.Text.Json;

namespace Corporate.Cashflow.Worker.Consumer
{
    public class CashflowConsumerService : BackgroundService
    {
        private readonly ILogger<CashflowConsumerService> _logger;
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceProvider _serviceProvider;

        public CashflowConsumerService(ILogger<CashflowConsumerService> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]!,
                GroupId = configuration["Kafka:GroupId"]!,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe(configuration["Kafka:Topic"]);
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    if (consumeResult != null)
                    {
                        _logger.LogInformation($"[Kafka] Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

                        var retryPolicy = CreateConcurrencyRetryPolicy();

                        var transaction = JsonSerializer.Deserialize<Transaction>(consumeResult.Message.Value);

                        var command = new ConsolidationCommand
                        {
                            AccountId = Guid.Parse(consumeResult.Message.Key),
                            Date = DateTime.UtcNow,
                            Amount = transaction!.Amount,
                            TransactionId = transaction.Id,
                            Description = transaction.Description,
                            TransactionType = transaction.TransactionType
                        };

                        using var scope = _serviceProvider.CreateScope();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        await retryPolicy.Execute(async () =>
                        {
                            await mediator.Send(command, stoppingToken);
                        });

                        _consumer.Commit(consumeResult);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"[Kafka] Error occurred: {ex.Error.Reason}");
                }
            }
        }


        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }

        private static RetryPolicy CreateConcurrencyRetryPolicy()
        {
            return Policy
                .Handle<DbUpdateConcurrencyException>()
                .WaitAndRetry(
                    [
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(3)
                    ],
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"Concurrency conflict detected. Retrying {retryCount}/3 after {timeSpan.TotalSeconds} seconds.");
                    }
                );
        }
    }
}
