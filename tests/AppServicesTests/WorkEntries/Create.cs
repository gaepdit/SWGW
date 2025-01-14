﻿using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Notifications;
using SWGW.AppServices.UserServices;
using SWGW.AppServices.WorkEntries;
using SWGW.AppServices.WorkEntries.CommandDto;
using SWGW.Domain.Entities.EntryTypes;
using SWGW.Domain.Entities.WorkEntries;
using SWGW.Domain.Identity;
using SWGW.TestData.Constants;

namespace AppServicesTests.WorkEntries;

public class Create
{
    [Test]
    public async Task OnSuccessfulInsert_ReturnsSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = new ApplicationUser { Id = Guid.Empty.ToString(), Email = TextData.ValidEmail };
        var workEntry = new WorkEntry(id) { ReceivedBy = user };

        var workEntryManagerMock = Substitute.For<IWorkEntryManager>();
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(user);

        workEntryManagerMock.Create(Arg.Any<ApplicationUser?>())
            .Returns(workEntry);

        userServiceMock.GetUserAsync(Arg.Any<string>())
            .Returns(user);
        userServiceMock.FindUserAsync(Arg.Any<string>())
            .Returns(user);

        var notificationMock = Substitute.For<INotificationService>();
        notificationMock
            .SendNotificationAsync(Arg.Any<Template>(), Arg.Any<string>(), Arg.Any<WorkEntry>(),
                Arg.Any<CancellationToken>())
            .Returns(NotificationResult.SuccessResult());

        var appService = new WorkEntryService(AppServicesTestsSetup.Mapper!, Substitute.For<IWorkEntryRepository>(),
            Substitute.For<IEntryTypeRepository>(), workEntryManagerMock, notificationMock, userServiceMock,
            Substitute.For<IAuthorizationService>());

        var item = new WorkEntryCreateDto { EntryTypeId = Guid.Empty, Notes = TextData.Phrase };

        // Act
        var result = await appService.CreateAsync(item, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.HasWarnings.Should().BeFalse();
        result.WorkEntryId.Should().Be(id);
    }
}
