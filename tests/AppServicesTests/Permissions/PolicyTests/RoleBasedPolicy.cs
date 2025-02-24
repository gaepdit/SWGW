﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.AppClaims;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.Domain.Identity;
using System.Security.Claims;

namespace AppServicesTests.Permissions.PolicyTests;

public class RoleBasedPolicy
{
    private IAuthorizationService _authorization = null!;

    [SetUp]
    public void SetUp() => _authorization = AuthorizationServiceBuilder.BuildAuthorizationService(collection =>
        collection.AddAuthorizationBuilder().AddPolicy(nameof(Policies.SiteMaintainer), Policies.SiteMaintainer));

    [Test]
    public async Task WhenAuthenticatedAndActiveAndDivisionManager_Succeeds()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[]
            {
                new(AppClaimTypes.ActiveUser, true.ToString()),
                new(ClaimTypes.Role, RoleName.SiteMaintenance),
            }, "Basic"));
        var result = await _authorization.Succeeded(user, Policies.SiteMaintainer);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotActive_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[]
            {
                new(ClaimTypes.Role, RoleName.SiteMaintenance),
            }, "Basic"));
        var result = await _authorization.Succeeded(user, Policies.SiteMaintainer);
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNotDivisionManager_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[]
            {
                new(AppClaimTypes.ActiveUser, true.ToString()),
            }, "Basic"));
        var result = await _authorization.Succeeded(user, Policies.SiteMaintainer);
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNotAuthenticated_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[]
            {
                new(AppClaimTypes.ActiveUser, true.ToString()),
                new(ClaimTypes.Role, RoleName.SiteMaintenance),
            }));
        var result = await _authorization.Succeeded(user, Policies.SiteMaintainer);
        result.Should().BeFalse();
    }
}
