using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Notifications;
using SWGW.AppServices.UserServices;
using SWGW.AppServices.Permits;
using SWGW.AppServices.Permits.CommandDto;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.Permits;
using SWGW.Domain.Identity;
using SWGW.TestData.Constants;


namespace AppServicesTests.Permits;

public class Create
{
    [Test]
    public async Task OnSuccessfulInsert_ReturnsSuccessfully()
    {
        // Arrange
        var id = 2;
        var user = new ApplicationUser { Id = Guid.Empty.ToString(), Email = TextData.ValidEmail };
        var permit = new Permit(id) { ReceivedBy = user };

        var permityManagerMock = Substitute.For<IPermitManager>();
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns(user);

        permityManagerMock.Create(Arg.Any<ApplicationUser?>())
            .Returns(permit);

        userServiceMock.GetUserAsync(Arg.Any<string>())
            .Returns(user);
        userServiceMock.FindUserAsync(Arg.Any<string>())
            .Returns(user);

        var notificationMock = Substitute.For<INotificationService>();
        notificationMock
            .SendNotificationAsync(Arg.Any<Template>(), Arg.Any<string>(), Arg.Any<Permit>(),
                Arg.Any<CancellationToken>())
            .Returns(NotificationResult.SuccessResult());

        var appService = new PermitService(AppServicesTestsSetup.Mapper!, Substitute.For<IPermitRepository>(),
            Substitute.For<IActionTypeRepository>(), permityManagerMock, notificationMock, userServiceMock,
            Substitute.For<IAuthorizationService>());

        var item = new PermitCreateDto { ActionTypeId = Guid.Empty, Notes = TextData.Phrase };

        // Act
        var result = await appService.CreateAsync(item, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.HasWarnings.Should().BeFalse();
        result.PermitId.Should().Be(id);
    }
}
