using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Application.Interfaces;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Transactions.GetAll
{
    public class Handler : IRequestHandler<GetAllTransactionsPaginatedQuery, Result<PagedResult<GetAllTransactionsPaginatedResponse>>>
    {
        private readonly ICashflowDbContext _context;

        public Handler(ICashflowDbContext context)
        {
            _context = context;
        }

        public Task<Result<PagedResult<GetAllTransactionsPaginatedResponse>>> Handle(GetAllTransactionsPaginatedQuery query, CancellationToken cancellationToken)
        {
            var queryable = _context.Transactions.AsQueryable();

            if(query.InitialDate.HasValue)
                queryable = queryable.Where(x => x.Date >= query.InitialDate);
            

            if (query.FinalDate.HasValue)
                queryable = queryable.Where(x => x.Date <= query.FinalDate);
            
            if(query.TransactionType.HasValue)
                queryable = queryable.Where(x => x.TransactionType == query.TransactionType);

            if (query.PaymentMethod.HasValue)
                queryable = queryable.Where(x => x.PaymentMethod == query.PaymentMethod);

            var total = queryable.Count();

            queryable = queryable
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize);

            var transactions = queryable.Select(x => new GetAllTransactionsPaginatedResponse 
            { 
                AccountId = x.AccountId, 
                Amount = x.Amount, 
                Date = x.Date, 
                Description = x.Description, 
                Id = x.Id, 
                PaymentMethod = x.PaymentMethod, 
                TransactionType = x.TransactionType 
            }).ToList();

            var result = new PagedResult<GetAllTransactionsPaginatedResponse>
            {
                Items = transactions,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalItems = total
            };

            return Task.FromResult(Result<PagedResult<GetAllTransactionsPaginatedResponse>>.Success(result));
        }
    }
}
