using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Application.Interfaces;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Balances.GetAll
{
    public class Handler : IRequestHandler<GetAllBalancesPaginatedQuery, Result<PagedResult<GetAllBalancesPaginatedResponse>>>
    {
        private readonly ICashflowDbContext _context;

        public Handler(ICashflowDbContext context)
        {
            _context = context;
        }

        public Task<Result<PagedResult<GetAllBalancesPaginatedResponse>>> Handle(GetAllBalancesPaginatedQuery query, CancellationToken cancellationToken)
        {
            var queryable = _context.AccountBalances.Where(x => x.AccountId == query.AccountId);

            if(query.InitialDate.HasValue)
                queryable = queryable.Where(x => x.Date >= query.InitialDate);
            

            if (query.FinalDate.HasValue)
                queryable = queryable.Where(x => x.Date <= query.FinalDate);

            var total = queryable.Count();

            queryable = queryable
                .OrderByDescending(x => x.Date)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize);

            var balances = queryable.Select(x => new GetAllBalancesPaginatedResponse 
            { 
                AccountId = x.AccountId, 
                Balance = x.Balance, 
                Date = x.Date,
                Inflows = x.Inflows,
                Outflows = x.Outflows
            }).ToList();

            var result = new PagedResult<GetAllBalancesPaginatedResponse>
            {
                Items = balances,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalItems = total
            };

            return Task.FromResult(Result<PagedResult<GetAllBalancesPaginatedResponse>>.Success(result));
        }
    }
}
