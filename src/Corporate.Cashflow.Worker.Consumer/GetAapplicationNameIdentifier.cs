using Corporate.Cashflow.Application.Interfaces;

namespace Corporate.Cashflow.Worker.Consumer
{
    public class GetAapplicationNameIdentifier : IGetIdentifier
    {
        public Guid GetAuthenticatedUserId()
        {
            return Guid.Parse("a9a7e17a-8264-4833-b52a-dd94d0afa4f2");
        }
    }
}
