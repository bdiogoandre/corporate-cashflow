using Confluent.Kafka;
using Corporate.Cashflow.Application.Interfaces;
using Corporate.Cashflow.Application.Results;
using Corporate.Cashflow.Domain.Transactions;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Corporate.Cashflow.Application.UseCases.Transactions.Create
{
    public class Handler : IRequestHandler<CreateTransactionCommand, Result<Guid>>
    {
        private readonly IProducer<string, string> _producer;
        private readonly ICashflowDbContext _context;
        private readonly string _topic;

        public Handler(IProducer<string, string> producer, ICashflowDbContext context, IConfiguration configuration)
        {
            _producer = producer;
            _context = context;
            _topic = configuration["Kafka:Topic"]!;
        }

        public async Task<Result<Guid>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            // Verificar Account


            var transaction = new Transaction
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                Description = request.Description!,
                TransactionType = request.TransactionType,
                Date = request.Date
            };

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync(cancellationToken);

            var result = await _producer.ProduceAsync(_topic, new Message<string, string>
            {
                Key = transaction.AccountId.ToString(),
                Value = JsonSerializer.Serialize(transaction),
                Timestamp = new Timestamp(DateTime.UtcNow)
            }, cancellationToken);

            if(result.Status != PersistenceStatus.Persisted)
                return Result<Guid>.Failure("Failed to produce message to Kafka");
            

            return Result<Guid>.Success(transaction.Id);
        }
    }
}
