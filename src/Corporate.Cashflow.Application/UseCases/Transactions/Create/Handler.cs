using Confluent.Kafka;
using Corporate.Cashflow.Application.Interfaces;
using Corporate.Cashflow.Domain.Transactions;
using MediatR;
using System.Text.Json;

namespace Corporate.Cashflow.Application.UseCases.Transactions.Create
{
    public class Handler : IRequestHandler<CreateTransactionCommand, CreateTransactionCommandResponse>
    {
        private readonly IProducer<string, string> _producer;
        private readonly ICashflowDbContext _context;

        public Handler(IProducer<string, string> producer, ICashflowDbContext context)
        {
            _producer = producer;
            _context = context;
        }

        public async Task<CreateTransactionCommandResponse> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            // Verificar Account

            // Validar

            var transaction = new TransactionEntity
            {
                AccountId = request.AccountId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system",
                Data = JsonSerializer.Serialize(new TransactionData { Amount = request.Amount, Description = request.Description }),
                Date = request.Date,
                Id = Guid.NewGuid()
            };

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync(cancellationToken);

            var result = await _producer.ProduceAsync("cashflow-events", new Message<string, string>
            {
                Key = transaction.AccountId.ToString(),
                Value = JsonSerializer.Serialize(new
                {
                    transaction.Id,
                    transaction.AccountId,
                    transaction.Date,
                    Data = JsonSerializer.Deserialize<TransactionData>(transaction.Data!)
                })
            }, cancellationToken);

            if(result.Status != PersistenceStatus.Persisted)
            {
                throw new Exception("Failed to produce message");
            }


            return new CreateTransactionCommandResponse(transaction.Id);
        }
    }
}
