using Corporate.Cashflow.Application.Common;
using ErrorOr;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Balances.GetByDate
{
    public record GetBalanceByDateQuery(Guid AccountId, DateOnly Date) : IRequest<ErrorOr<GetBalanceByDateResponse>>;

    public class GetBalanceByDateResponse
    {
        public Guid AccountId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Inflows { get; set; }
        public decimal Outflows { get; set; }
        public decimal Balance { get; set; }
    }
}
