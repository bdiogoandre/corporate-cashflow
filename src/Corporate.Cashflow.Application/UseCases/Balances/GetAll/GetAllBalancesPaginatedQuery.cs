using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Domain.Enums;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Balances.GetAll
{
    public class GetAllBalancesPaginatedQuery : PaginationFilter, IRequest<Result<PagedResult<GetAllBalancesPaginatedResponse>>>
    {
        public Guid AccountId { get; set; }
        public DateOnly? InitialDate { get; set; }
        public DateOnly? FinalDate { get; set; }
    }

    public class GetAllBalancesPaginatedResponse
    {
        public Guid AccountId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Inflows { get; set; }
        public decimal Outflows { get; set; }
        public decimal Balance { get; set; }
    }
}
