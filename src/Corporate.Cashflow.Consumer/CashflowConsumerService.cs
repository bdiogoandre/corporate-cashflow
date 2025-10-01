using Confluent.Kafka;
using Corporate.Cashflow.Application.UseCases.Balances.Consolidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Corporate.Cashflow.Consumer
{
    public class CashflowConsumerService : BackgroundService
    {
        private readonly ILogger<CashflowConsumerService> _logger;
        private readonly string _topic = "cashflow-topic";
        private readonly IConsumer<string, string> _consumer;
        private readonly IMediator _mediator;

        public CashflowConsumerService(ILogger<CashflowConsumerService> logger, IMediator mediator)
        {
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "cashflow-consumers",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe(_topic);
            _mediator = mediator;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                while(!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(stoppingToken);
                        if (consumeResult != null)
                        {
                            _logger.LogInformation($"[Kafka] Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

                            var retryInConcurencyFailure = Policy
                                .Handle<DbUpdateConcurrencyException>()
                                .WaitAndRetry([TimeSpan.FromSeconds(1),TimeSpan.FromSeconds(2),TimeSpan.FromSeconds(3)], (exception, timeSpan, retryCount, context) =>
                                    {
                                        _logger.LogWarning($"[Kafka] Concurrency conflict detected. Retrying {retryCount}/3 after {timeSpan.TotalSeconds} seconds.");
                                    });

                            retryInConcurencyFailure.Execute(async () =>
                            {
                                var command = new ConsolidationCommand
                                {
                                    AccountId = Guid.Parse(consumeResult.Message.Key),
                                    Date = DateTime.UtcNow,
                                    Data = consumeResult.Message.Value
                                };

                                await _mediator.Send(command, stoppingToken);
                            });

                            _consumer.Commit(consumeResult);
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"[Kafka] Error occurred: {ex.Error.Reason}");
                    }
                }
            }, stoppingToken);
        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
