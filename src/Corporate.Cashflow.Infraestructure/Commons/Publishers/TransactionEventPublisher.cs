using RabbitMQ.Stream.Client.AMQP;
using RabbitMQ.Stream.Client.Reliable;
using RabbitMQ.Stream.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Corporate.Cashflow.Domain.Transactions;

namespace Corporate.Cashflow.Infraestructure.Commons.Publishers
{
    public class TransactionEventPublisher
    {
        private readonly Producer _producer;

        public TransactionEventPublisher(Producer producer)
        {
            _producer = producer;
        }

        public async Task PublishAsync(TransactionCreated evt)
        {
            var message = new Message(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt)))
            {
                Properties = new Properties
                {
                    MessageId = Guid.NewGuid().ToString(),

                },
                ApplicationProperties = new ApplicationProperties
                {
                    { "EventType", nameof(TransactionCreated) },
                    { "AccountId", evt.AccountId.ToString() },
                    { "AccountId", evt.AccountId },
                    { "Date", evt.Date },
                    { "Sequence", evt.Sequence }
                },
            };

            // Partition key ensures ordering by merchant
            await _producer.Send(new List<Message> { message }, partitionKey: evt.AccountId.ToString());
        }
    }
}
