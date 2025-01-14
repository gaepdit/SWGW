﻿using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Offices;
using SWGW.AppServices.UserServices;
using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Identity;
using SWGW.TestData.Constants;

namespace AppServicesTests.Offices;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        // Arrange
        var item = new Office(Guid.NewGuid(), TextData.ValidName);

        var managerMock = Substitute.For<IOfficeManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Is((string?)null), Arg.Any<CancellationToken>()).Returns(item);

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync().Returns((ApplicationUser?)null);

        var appService = new OfficeService(AppServicesTestsSetup.Mapper!, Substitute.For<IOfficeRepository>(),
            managerMock, userServiceMock, Substitute.For<IAuthorizationService>());

        // Act
        var result = await appService.CreateAsync(TextData.ValidName);

        // Assert
        result.Should().Be(item.Id);
    }
}
