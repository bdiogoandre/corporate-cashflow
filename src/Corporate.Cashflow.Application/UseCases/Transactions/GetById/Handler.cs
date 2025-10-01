using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Application.Interfaces;
using ErrorOr;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Transactions.GetById
{
    public class Handler : IRequestHandler<GetTransactionByIdQuery, ErrorOr<GetTransactionByIdResponse>>
    {
        private readonly ICashflowDbContext _context;

        public Handler(ICashflowDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<GetTransactionByIdResponse>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Transactions.FindAsync(request.Id, cancellationToken);
            if (result == null)
            {
                return Error.NotFound("Transaction not found");
            }

            return new GetTransactionByIdResponse
            {
                Id = result.Id,
                AccountId = result.AccountId,
                Amount = result.Amount,
                Date = result.Date,
                Description = result.Description,
                TransactionType = result.TransactionType
            };
        }
    }
}
