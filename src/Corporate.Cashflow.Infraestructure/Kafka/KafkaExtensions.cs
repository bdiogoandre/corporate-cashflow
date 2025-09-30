using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Corporate.Cashflow.Infraestructure.Kafka
{
    public static class KafkaExtensions
    {
        public static IServiceCollection AddKafka(this IServiceCollection services, string bootstrapServers)
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            var producer = new ProducerBuilder<string, string>(config).Build();
            services.AddSingleton(producer);
            return services;
        }
    }

}
