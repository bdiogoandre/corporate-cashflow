using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Application.Interfaces;
using Corporate.Cashflow.Domain.Accounts;
using ErrorOr;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Accounts.Create
{
    public class Handler : IRequestHandler<CreateAccountCommand, ErrorOr<Guid>>
    {
        private readonly ICashflowDbContext _context;
        private readonly IGetIdentifier _getIdentifier;

        public Handler(ICashflowDbContext context, IGetIdentifier getIdentifier)
        {
            _context = context;
            _getIdentifier = getIdentifier;
        }

        public async Task<ErrorOr<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                UserId = _getIdentifier.GetAuthenticatedUserId(),
                Name = request.Name!,
                Currency = request.Currency
            };

            _context.Accounts.Add(account);

            await _context.SaveChangesAsync(cancellationToken);

            return account.Id;
        }
    }
}
