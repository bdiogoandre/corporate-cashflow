using Corporate.Cashflow.Domain;
using System.Security.Claims;

namespace Corporate.Cashflow.Worker.Consumer
{
    public class GetAapplicationNameIdentifier : IGetIdentifier
    {
        public string GetAuthenticatedUserAccountId()
        {
            return "cashflow-worker-consumer";
        }
    }
}
