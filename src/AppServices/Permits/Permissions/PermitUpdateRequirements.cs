using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Permits.CommandDto;
using System.Security.Claims;

namespace SWGW.AppServices.Permits.Permissions;

public class PermitUpdateRequirements :
    AuthorizationHandler<PermitUpdateRequirements, PermitUpdateDto>, IAuthorizationRequirement
{
    private ClaimsPrincipal _user = null!;
    private PermitUpdateDto _resource = null!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermitUpdateRequirements requirements,
        PermitUpdateDto resource)
    {
        _user = context.User;
        _resource = resource;

        if (UserCanEditDetails())
            context.Succeed(requirements);

        return Task.FromResult(0);
    }

    private bool UserCanEditDetails() => IsOpen() && _user.IsManager();
    private bool IsOpen() => _resource is { Closed: false, IsDeleted: false };
}
