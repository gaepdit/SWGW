using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.UserServices;
using SWGW.Domain.Entities.ActionTypes;
using System.Linq.Expressions;

namespace AppServicesTests.ActionTypes;

public class GetActiveListItems
{
    [Test]
    public async Task GetAsListItems_ReturnsListOfListItems()
    {
        // Arrange
        var itemList = new List<ActionType>
        {
            new(Guid.Empty, "One"),
            new(Guid.Empty, "Two"),
        };

        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<Expression<Func<ActionType, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(itemList);

        var managerMock = Substitute.For<IActionTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ActionTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        // Act
        var result = await appService.GetAsListItemsAsync();

        // Assert
        result.Should().BeEquivalentTo(itemList);
    }
}
