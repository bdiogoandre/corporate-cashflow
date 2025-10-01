using Corporate.Cashflow.Application.Interfaces;
using Corporate.Cashflow.Domain;
using System.Security.Claims;

namespace Corporate.CashFlow.Api
{
    public class GetAuthenticatedUserAccountIdentifier : IGetIdentifier
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAuthenticatedUserAccountIdentifier(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetAuthenticatedUserId()
        {
            return ClaimsPrincipalExtensions.GetUserIdAsValidatedGuid(_httpContextAccessor.HttpContext!.User);
        }
    }
}
