﻿using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Notifications;
using SWGW.AppServices.Permits;
using SWGW.AppServices.UserServices;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.Permits;
using System.Security.Claims;

namespace AppServicesTests.Permits;

public class Find
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new Permit(1);

        var repoMock = Substitute.For<IPermitRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(item);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var appService = new PermitService(AppServicesTestsSetup.Mapper!, repoMock,
            Substitute.For<IActionTypeRepository>(), Substitute.For<PermitManager>(),
            Substitute.For<INotificationService>(), Substitute.For<IUserService>(), authorizationMock);

        // Act
        var result = await appService.FindAsync(item.Id);

        // Assert
        result.Should().BeEquivalentTo(item);
    }


    [Test]
    public async Task WhenNoItemExists_ReturnsNull()
    {
        // Arrange
        var repoMock = Substitute.For<IPermitRepository>();
        repoMock.FindIncludeAllAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns((Permit?)null);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var appService = new PermitService(AppServicesTestsSetup.Mapper!, Substitute.For<IPermitRepository>(),
            Substitute.For<IActionTypeRepository>(), Substitute.For<PermitManager>(),
            Substitute.For<INotificationService>(), Substitute.For<IUserService>(), authorizationMock);

        // Act
        var result = await appService.FindAsync(0);

        // Assert
        result.Should().BeNull();
    }
}
