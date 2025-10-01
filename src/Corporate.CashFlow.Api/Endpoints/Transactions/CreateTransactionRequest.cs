using Corporate.Cashflow.Domain.Enums;

namespace Corporate.CashFlow.Api.Endpoints.Transactions
{
    public class CreateTransactionRequest
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public ETransactionType TransactionType { get; set; }
        public EPaymentMethod PaymentMethod { get; set; }
    }
}
