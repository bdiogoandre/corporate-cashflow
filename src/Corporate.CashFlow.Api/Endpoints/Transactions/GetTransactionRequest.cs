using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Domain.Enums;

namespace Corporate.CashFlow.Api.Endpoints.Transactions
{
    public class GetTransactionRequest : PaginationFilter
    {
        public DateTimeOffset? InitialDate { get; set; }
        public DateTimeOffset? FinalDate { get; set; }
        public ETransactionType? TransactionType { get; set; }
        public EPaymentMethod? PaymentMethod { get; set; }
    }
}
