using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporate.Cashflow.Application.UseCases.Balances.GetByDate
{
    public class Handler : IRequestHandler<GetBalanceByDateQuery, Result<GetBalanceByDateResponse>>
    {
        private readonly ICashflowDbContext _context;

        public Handler(ICashflowDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetBalanceByDateResponse>> Handle(GetBalanceByDateQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.AccountBalances
                .Where(x => x.AccountId == request.AccountId && x.Date == request.Date)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                return Result<GetBalanceByDateResponse>.Failure("Balance not found for the given date.");

            var response = new GetBalanceByDateResponse
            {
                AccountId = result.AccountId,
                Date = result.Date,
                Inflows = result.Inflows,
                Outflows = result.Outflows,
                Balance = result.Balance
            };

            return Result<GetBalanceByDateResponse>.Success(response);
        }
    }
}
