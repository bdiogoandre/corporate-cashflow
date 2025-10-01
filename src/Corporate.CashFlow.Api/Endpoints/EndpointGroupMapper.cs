using CashFlow.Api.Endpoints;
using Corporate.CashFlow.Api.Endpoints.Accounts;
using Corporate.CashFlow.Api.Endpoints.Balance;
using Corporate.CashFlow.Api.Endpoints.Transactions;

namespace Corporate.CashFlow.Api.Endpoints
{
    public static class EndpointGroupMapper
    {
        public static void MapEndpointGroups(this WebApplication app)
        {
            var root = app.MapGroup("/api")
                                .RequireAuthorization()
                                .AddEndpointFilter<EndpointFilters>()
                                .ProducesValidationProblem();


            var accounts = root.MapGroup("/accounts")
                               .MapAccountsEndpoint();

            accounts.MapGroup("{accountId:guid}/transactions")
                    .MapTransactionsEndpoints();
            
            accounts.MapGroup("{accountId:guid}/balances")
                    .MapBalancesEndpoints();
        }
    }
}
