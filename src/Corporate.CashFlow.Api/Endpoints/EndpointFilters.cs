using Corporate.CashFlow.Api;

namespace CashFlow.Api.Endpoints;

public class EndpointFilters : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var user = context.HttpContext.User;

        if (user.GetUserIdAsNullableGuid() is null)
            return Results.StatusCode(StatusCodes.Status403Forbidden);

        return await next(context);
    }
}
