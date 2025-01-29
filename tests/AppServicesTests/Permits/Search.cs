using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Notifications;
using SWGW.AppServices.UserServices;
using SWGW.AppServices.Permits;
using SWGW.AppServices.Permits.QueryDto;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.Permits;
using SWGW.TestData;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Security.Claims;

namespace AppServicesTests.Permits;

public class Search
{
    [Test]
    public async Task WhenItemsExist_ReturnsPagedList()
    {
        // Arrange
        var itemList = new ReadOnlyCollection<Permit>(PermitData.GetPermits.ToList());
        var count = PermitData.GetPermits.Count();

        var paging = new PaginatedRequest(1, 100);

        var repoMock = Substitute.For<IPermitRepository>();
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<Permit, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        repoMock.CountAsync(Arg.Any<Expression<Func<Permit, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var appService = new PermitService(AppServicesTestsSetup.Mapper!, repoMock,
            Substitute.For<IActionTypeRepository>(), Substitute.For<PermitManager>(),
            Substitute.For<INotificationService>(), Substitute.For<IUserService>(), authorizationMock);

        // Act
        var result = await appService.SearchAsync(new PermitSearchDto(), paging, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEquivalentTo(itemList);
        result.CurrentCount.Should().Be(count);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyPagedList()
    {
        // Arrange
        var itemList = new ReadOnlyCollection<Permit>(new List<Permit>());
        const int count = 0;

        var paging = new PaginatedRequest(1, 100);

        var repoMock = Substitute.For<IPermitRepository>();
        repoMock.GetPagedListAsync(Arg.Any<Expression<Func<Permit, bool>>>(),
                Arg.Any<PaginatedRequest>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        repoMock.CountAsync(
                Arg.Any<Expression<Func<Permit, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(count);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), resource: Arg.Any<object?>(),
                requirements: Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var appService = new PermitService(AppServicesTestsSetup.Mapper!, repoMock,
            Substitute.For<IActionTypeRepository>(), Substitute.For<PermitManager>(),
            Substitute.For<INotificationService>(), Substitute.For<IUserService>(),
            authorizationMock);

        // Act
        var result = await appService.SearchAsync(new PermitSearchDto(), paging, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.Items.Should().BeEmpty();
        result.CurrentCount.Should().Be(count);
    }
}
