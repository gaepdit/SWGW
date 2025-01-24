using SWGW.AppServices.Permissions.AppClaims;
using SWGW.Domain.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;

namespace SWGW.AppServices.Permissions.Helpers;

public static class PrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Email);

    public static string GetGivenName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;

    public static string GetFamilyName(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;

    public static bool HasRealClaim(this ClaimsPrincipal principal, string type, [NotNullWhen(true)] string? value) =>
        value is not null && principal.HasClaim(type, value);

    internal static bool IsActive(this ClaimsPrincipal principal) =>
        principal.HasClaim(AppClaimTypes.ActiveUser, true.ToString());
    private static bool IsInOneOfRoles(this IPrincipal principal, IEnumerable<string> roles) =>
    roles.Any(principal.IsInRole);
    private static bool IsInRoles(this IPrincipal principal, IEnumerable<string> roles) =>
        roles.Any(principal.IsInRole);

    internal static bool IsManager(this IPrincipal principal) =>
      principal.IsInOneOfRoles([RoleName.ProgramManager, RoleName.UnitManager]);
    internal static bool IsProgramManager(this IPrincipal principal) =>
        principal.IsInRole(RoleName.ProgramManager);

    internal static bool IsUnitManager(this IPrincipal principal) =>
      principal.IsInRole(RoleName.UnitManager);

    internal static bool IsEngineer(this IPrincipal principal) =>
     principal.IsInRole(RoleName.Engineer);

    internal static bool IsGeologist(this IPrincipal principal) =>
     principal.IsInRole(RoleName.Geologist);

    internal static bool IsSiteMaintainer(this IPrincipal principal) =>
        principal.IsInRole(RoleName.SiteMaintenance);

    internal static bool IsStaff(this IPrincipal principal) =>
        principal.IsInRoles([RoleName.Staff, RoleName.ProgramManager]);

    internal static bool IsUserAdmin(this IPrincipal principal) =>
        principal.IsInOneOfRoles([RoleName.UserAdmin, RoleName.SuperUserAdmin, RoleName.ProgramManager]);
    internal static bool IsSuperUserAdmin(this IPrincipal principal) =>
    principal.IsInOneOfRoles([RoleName.SuperUserAdmin, RoleName.ProgramManager]);
}
