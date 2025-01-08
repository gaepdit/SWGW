using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Permissions.Helpers;

namespace SWGW.AppServices.Permissions.Requirements;

internal class ManagerRequirement :
    AuthorizationHandler<ManagerRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ManagerRequirement requirement)
    {
        if (context.User.IsManager())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
