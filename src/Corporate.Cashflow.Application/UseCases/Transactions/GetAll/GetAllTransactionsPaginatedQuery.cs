using Corporate.Cashflow.Application.Common;
using Corporate.Cashflow.Domain.Enums;
using ErrorOr;
using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Transactions.GetAll
{
    public class GetAllTransactionsPaginatedQuery : PaginationFilter, IRequest<ErrorOr<PagedResult<GetAllTransactionsPaginatedResponse>>>
    {
        public Guid AccountId { get; set; }
        public DateTimeOffset? InitialDate { get; set; }
        public DateTimeOffset? FinalDate { get; set; }
        public ETransactionType? TransactionType { get; set; }
        public EPaymentMethod? PaymentMethod { get; set; }
    }

    public class GetAllTransactionsPaginatedResponse
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset Date { get; set; }
        public ETransactionType TransactionType { get; set; }
        public EPaymentMethod PaymentMethod { get; set; }
    }
}
