using Corporate.Cashflow.Application.Common;

namespace Corporate.CashFlow.Api.Endpoints.Balance
{
    public class GetAllBalancesPaginatedRequest : PaginationFilter
    {
        public DateOnly? InitialDate { get; set; }
        public DateOnly? FinalDate { get; set; }
    }
}
