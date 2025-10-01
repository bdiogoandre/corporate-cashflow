using Corporate.Cashflow.Domain.Enums;

namespace Corporate.CashFlow.Api.Endpoints.Accounts
{
    public class CreateAccountRequest
    {
        public string? Name { get; set; }
        public ECurrency Currency { get; set; }
    }
}
