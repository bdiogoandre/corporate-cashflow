using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;

namespace Corporate.Cashflow.Infraestructure.Kafka
{
    public static class KafkaSetup
    {
        public static async Task EnsureTopicExists(
            string bootstrapServers,
            string topicName,
            int numPartitions = 1,
            short replicationFactor = 1)
        {
            using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();

            try
            {
                var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));

                if (metadata.Topics.Any(t => t.Topic == topicName))
                {
                    Console.WriteLine($"[Kafka] Topic {topicName} already exists.");
                    return;
                }

                await adminClient.CreateTopicsAsync(
                [
                    new() {
                        Name = topicName,
                        NumPartitions = numPartitions,
                        ReplicationFactor = replicationFactor
                    }
                ]);

                Console.WriteLine($"[Kafka] Topic {topicName} created with {numPartitions} partitions.");
            }
            catch (CreateTopicsException e)
            {
                if (e.Results[0].Error.Code == ErrorCode.TopicAlreadyExists)
                    Console.WriteLine($"[Kafka] Topic {topicName} already exists.");
                else
                    throw;
            }
        }
    }
}
