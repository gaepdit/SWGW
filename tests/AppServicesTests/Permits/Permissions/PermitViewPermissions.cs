using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Perimits.Permissions;
using SWGW.AppServices.Perimits.QueryDto;
using SWGW.Domain.Identity;
using System.Security.Claims;

namespace AppServicesTests.Perimits.Permissions;

public class WorkEntryViewPermissions
{
    [Test]
    public async Task ManageDeletions_WhenAllowed_Succeeds()
    {
        var requirements = new[] { PermitOperation.ManageDeletions };
        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.ProgramManager) },
            "Basic"));
        var resource = new PermitViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new PermitViewRequirements();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAuthenticated_DoesNotSucceed()
    {
        var requirements = new[] { PermitOperation.ManageDeletions };
        // This `ClaimsPrincipal` is not authenticated.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.ProgramManager) }));
        var resource = new PermitViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new PermitViewRequirements();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAllowed_DoesNotSucceed()
    {
        var requirements = new[] { PermitOperation.ManageDeletions };
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var resource = new PermitViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new PermitViewRequirements();

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }
}
