using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SWGW.AppServices.Perimits.Permissions;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.AppClaims;
using System.Diagnostics.CodeAnalysis;

namespace SWGW.AppServices.RegisterServices;

[SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out")]
public static class AuthorizationHandlers
{
    public static void AddAuthorizationHandlers(this IServiceCollection services)
    {
        services.AddAuthorizationPolicies();

        // Resource/operation-based permission handlers, e.g.:
        // var canAssign = await authorization.Succeeded(User, permitView, permitOperation.EditPermit);

        services.AddSingleton<IAuthorizationHandler, PermitViewRequirements>();

        // Add claims transformations
        services.AddScoped<IClaimsTransformation, AppClaimsTransformation>();
    }
}
