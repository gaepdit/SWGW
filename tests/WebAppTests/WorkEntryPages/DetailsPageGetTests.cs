using SWGW.AppServices.Permits;
using SWGW.AppServices.Permits.Permissions;
using SWGW.AppServices.Permits.QueryDto;

namespace WebAppTests.PermitPages;

public class DetailsPageGetTests
{
    private static readonly PermitViewDto ItemTest = new() { Id = 0 };

    [Test]
    public async Task OnGetReturnsWithCorrectPermissions()
    {
        // Arrange
        var permitService = Substitute.For<IPermitService>();
        permitService.FindAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(ItemTest);

        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Is<IAuthorizationRequirement[]>(x => x.Contains(PermitOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Success());
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Is<IAuthorizationRequirement[]>(x => !x.Contains(PermitOperation.ManageDeletions)))
            .Returns(AuthorizationResult.Failed());

        var page = PageModelHelpers.BuildDetailsPageModel(permitService, authorizationService: authorizationMock);
        page.TempData = WebAppTestsSetup.PageTempData();
        page.PageContext = WebAppTestsSetup.PageContextWithUser();

        // Act
        await page.OnGetAsync(ItemTest.Id);

        // Assert
        using var scope = new AssertionScope();
        page.ItemView.Should().BeEquivalentTo(ItemTest);
        page.UserCan.Should().NotBeEmpty();
        page.UserCan[PermitOperation.ManageDeletions].Should().BeTrue();
        page.UserCan[PermitOperation.ViewDeletedActions].Should().BeFalse();
    }

    [Test]
    public async Task OnGetAsync_NullId_ReturnsRedirectToPageResult()
    {
        var page = PageModelHelpers.BuildDetailsPageModel();
        var result = await page.OnGetAsync(null);
        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Test]
    public async Task OnGetAsync_CaseNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        var id = 0;

        var permitService = Substitute.For<IPermitService>();
        permitService.FindAsync(id).Returns((PermitViewDto?)null);

        var page = PageModelHelpers.BuildDetailsPageModel(permitService);

        // Act
        var result = await page.OnGetAsync(id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
