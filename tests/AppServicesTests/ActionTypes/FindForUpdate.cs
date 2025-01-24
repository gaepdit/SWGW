using AutoMapper;
using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.UserServices;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.TestData.Constants;

namespace AppServicesTests.ActionTypes;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var item = new ActionType(Guid.Empty, TextData.ValidName);
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.FindAsync(item.Id, Arg.Any<CancellationToken>())
            .Returns(item);
        var managerMock = Substitute.For<IActionTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ActionTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>())
            .Returns((ActionType?)null);
        var managerMock = Substitute.For<IActionTypeManager>();
        var mapperMock = Substitute.For<IMapper>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ActionTypeService(mapperMock, repoMock, managerMock, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
