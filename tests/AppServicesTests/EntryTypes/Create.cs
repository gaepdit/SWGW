﻿using SWGW.AppServices.EntryTypes;
using SWGW.AppServices.UserServices;
using SWGW.Domain.Entities.EntryTypes;
using SWGW.Domain.Identity;
using SWGW.TestData.Constants;

namespace AppServicesTests.EntryTypes;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new EntryType(Guid.NewGuid(), TextData.ValidName);
        var repoMock = Substitute.For<IEntryTypeRepository>();
        var managerMock = Substitute.For<IEntryTypeManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns((ApplicationUser?)null);
        var appService = new EntryTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        var result = await appService.CreateAsync(item.Name);

        result.Should().Be(item.Id);
    }
}
