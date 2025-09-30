using Corporate.Cashflow.Application.Interfaces;
using Corporate.Cashflow.Domain.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Corporate.Cashflow.Application.UseCases.Balances.Consolidation
{
    public class ConsolidationHandler : IRequestHandler<ConsolidationCommand>
    {
        private readonly ICashflowDbContext _context;

        public ConsolidationHandler(ICashflowDbContext context)
        {
            _context = context;
        }

        public async Task Handle(ConsolidationCommand request, CancellationToken cancellationToken)
        { 
            var balance = await _context.AccountBalances
                .FirstOrDefaultAsync(x => x.AccountId == request.AccountId && x.Date == DateOnly.FromDateTime(request.Date.Date), cancellationToken);

            var transactionData = JsonSerializer.Deserialize<TransactionData>(request.Data!);

            if (balance == null)
            {
                balance = new Domain.Account.AccountBalanceEntity
                {
                    AccountId = request.AccountId,
                    Date = DateOnly.FromDateTime(request.Date.Date),
                    Inflows = 0,
                    Outflows = 0,
                    Balance = 0
                };
                _context.AccountBalances.Add(balance);
            }

            if (transactionData.TransactionType == ETransactionType.Inflow)
            {
                balance.Inflows += transactionData.Amount;
                balance.Balance += transactionData.Amount;
            }
            else if (transactionData.TransactionType == ETransactionType.Outflow)
            {
                balance.Outflows += transactionData.Amount;
                balance.Balance -= transactionData.Amount;
            }

            // Verificar a inclusão do offset aqui


            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
