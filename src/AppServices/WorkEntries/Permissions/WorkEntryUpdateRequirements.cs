using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.WorkEntries.CommandDto;
using System.Security.Claims;

namespace SWGW.AppServices.WorkEntries.Permissions;

public class WorkEntryUpdateRequirements :
    AuthorizationHandler<WorkEntryUpdateRequirements, WorkEntryUpdateDto>, IAuthorizationRequirement
{
    private ClaimsPrincipal _user = null!;
    private WorkEntryUpdateDto _resource = null!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        WorkEntryUpdateRequirements requirements,
        WorkEntryUpdateDto resource)
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
