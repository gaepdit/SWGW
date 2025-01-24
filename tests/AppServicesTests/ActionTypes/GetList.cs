using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.UserServices;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.TestData.Constants;

namespace AppServicesTests.ActionTypes;

public class GetList
{
    [Test]
    public async Task ReturnsViewDtoList()
    {
        var itemList = new List<ActionType> { new(Guid.Empty, TextData.ValidName) };
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>())
            .Returns(itemList);
        var managerMock = Substitute.For<IActionTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ActionTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
