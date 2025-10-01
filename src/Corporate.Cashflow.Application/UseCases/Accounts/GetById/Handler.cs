using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Application.Interfaces;
using ErrorOr;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Accounts.GetById
{
    public class Handler : IRequestHandler<GetAccountByIdQuery, ErrorOr<GetAccountByIdResponse>>
    {
        private readonly ICashflowDbContext _context;

        public Handler(ICashflowDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<GetAccountByIdResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FindAsync(request.Id, cancellationToken);
            if (account == null)
                return Error.NotFound("Account not found");

            var response = new GetAccountByIdResponse 
            { 
                Id = account.Id,
                Name = account.Name,
                Currency = account.Currency,
                UserId = account.UserId
            };

            return response;
        }
    }
}
