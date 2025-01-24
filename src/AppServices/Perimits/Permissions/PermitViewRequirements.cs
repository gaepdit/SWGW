using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Perimits.QueryDto;
using System.Security.Claims;

namespace SWGW.AppServices.Perimits.Permissions;

internal class PermitViewRequirements :
    AuthorizationHandler<PermitOperation, PermitViewDto>
{
    private ClaimsPrincipal _user = null!;
    private PermitViewDto _resource = null!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermitOperation requirement,
        PermitViewDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
            return Task.FromResult(0);

        _user = context.User;
        _resource = resource;

        var success = requirement.Name switch
        {
            nameof(PermitOperation.EditPermit) => UserCanEditDetails(),
            nameof(PermitOperation.ManageDeletions) => _user.IsManager(),
            nameof(PermitOperation.ViewDeletedActions) => _user.IsManager(),
            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    // Permissions methods
    private bool UserCanEditDetails() => IsOpen() && _user.IsManager();
    private bool IsOpen() => _resource is { Closed: false, IsDeleted: false };
}
