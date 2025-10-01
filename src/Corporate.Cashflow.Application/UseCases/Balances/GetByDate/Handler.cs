using Corporate.Cashflow.Application.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Cashflow.Application.UseCases.Balances.GetByDate
{
    public class Handler : IRequestHandler<GetBalanceByDateQuery, ErrorOr<GetBalanceByDateResponse>>
    {
        private readonly ICashflowDbContext _context;

        public Handler(ICashflowDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<GetBalanceByDateResponse>> Handle(GetBalanceByDateQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.AccountBalances
                .Where(x => x.AccountId == request.AccountId && x.Date == request.Date)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                return Error.NotFound("Balance not found for the given date.");

            var response = new GetBalanceByDateResponse
            {
                AccountId = result.AccountId,
                Date = result.Date,
                Inflows = result.Inflows,
                Outflows = result.Outflows,
                Balance = result.Balance
            };

            return response;
        }
    }
}
