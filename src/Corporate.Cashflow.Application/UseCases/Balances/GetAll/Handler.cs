using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Application.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Corporate.Cashflow.Application.UseCases.Balances.GetAll
{
    public class Handler : IRequestHandler<GetAllBalancesPaginatedQuery, ErrorOr<PagedResult<GetAllBalancesPaginatedResponse>>>
    {
        private readonly ICashflowDbContext _context;

        public Handler(ICashflowDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<PagedResult<GetAllBalancesPaginatedResponse>>> Handle(GetAllBalancesPaginatedQuery query, CancellationToken cancellationToken)
        {
            var queryable = _context.AccountBalances.Where(x => x.AccountId == query.AccountId);

            if(query.InitialDate.HasValue)
                queryable = queryable.Where(x => x.Date >= query.InitialDate);
            

            if (query.FinalDate.HasValue)
                queryable = queryable.Where(x => x.Date <= query.FinalDate);

            var total = queryable.Count();

            var result = await queryable
                .OrderByDescending(x => x.Date)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var balances = result.Select(x => new GetAllBalancesPaginatedResponse 
            { 
                AccountId = x.AccountId, 
                Balance = x.Balance, 
                Date = x.Date,
                Inflows = x.Inflows,
                Outflows = x.Outflows
            }).ToList();

            return new PagedResult<GetAllBalancesPaginatedResponse>
            {
                Items = balances,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalItems = total
            };

        }
    }
}
