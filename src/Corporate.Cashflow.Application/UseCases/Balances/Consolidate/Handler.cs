using Corporate.Cashflow.Application.Interfaces;
using Corporate.Cashflow.Domain.Accounts;
using Corporate.Cashflow.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Cashflow.Application.UseCases.Balances.Consolidate
{
    public class Handler : IRequestHandler<ConsolidationCommand>
    {
        private readonly ICashflowDbContext _context;

        public Handler(ICashflowDbContext context)
        {
            _context = context;
        }

        public async Task Handle(ConsolidationCommand request, CancellationToken cancellationToken)
        { 
            var balance = await _context.AccountBalances
                .FirstOrDefaultAsync(x => x.AccountId == request.AccountId && x.Date == DateOnly.FromDateTime(request.Date.Date), cancellationToken);

            if (balance == null)
            {
                balance = new AccountBalance
                {
                    AccountId = request.AccountId,
                    Date = DateOnly.FromDateTime(request.Date.Date),
                    Inflows = 0,
                    Outflows = 0,
                    Balance = 0,
                    LastTransactionId = request.TransactionId
                };

                _context.AccountBalances.Add(balance);
            }
            else
            {
                // Idempotency check
                if (balance.LastTransactionId == request.TransactionId) return;
            }


            balance.LastTransactionId = request.TransactionId;

            if (request.TransactionType == ETransactionType.Inflow)
                balance.CalculateInflow(request.Amount);
            else
                balance.CalculateOutflow(request.Amount);


            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
