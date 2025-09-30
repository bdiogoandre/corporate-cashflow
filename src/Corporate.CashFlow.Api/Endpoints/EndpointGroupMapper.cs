using CashFlow.Api.Endpoints;
using Corporate.CashFlow.Api.Endpoints.Transactions;

namespace Corporate.CashFlow.Api.Endpoints
{
    public class EndpointGroupMapper
    {
        public static void MapEndpointGroups(WebApplication app)
        {
            var rootRoutes = app.MapGroup("/api")
                                .RequireAuthorization()
                                .AddEndpointFilter<EndpointFilters>()
                                .ProducesValidationProblem();


            var accounts = rootRoutes.MapGroup("/accounts");

            accounts.MapGroup("{accountId:guid}/transactions")
                    .MapCreateTransactionEndpoints();
        }
    }
}
