﻿using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Permissions.Helpers;

namespace SWGW.AppServices.Permissions.Requirements;

internal class SiteMaintainerRequirement :
    AuthorizationHandler<SiteMaintainerRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SiteMaintainerRequirement requirement)
    {
        if (context.User.IsSiteMaintainer())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
