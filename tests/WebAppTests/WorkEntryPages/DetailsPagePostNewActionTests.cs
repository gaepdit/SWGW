using SWGW.AppServices.PermitActions.Dto;
using SWGW.AppServices.Permits;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permits.QueryDto;

namespace WebAppTests.PermitPages;

public class DetailsPagePostNewActionTests
{
    [Test]
    public async Task OnPostAsync_NullId_ReturnsRedirectToPageResult()
    {
        // Arrange
        var id = 1;
        var dto = new ActionCreateDto(id);
        var page = PageModelHelpers.BuildDetailsPageModel();

        // Act
        var result = await page.OnPostNewActionAsync(null, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_PermitNotFound_ReturnsBadRequestResult()
    {
        // Arrange
        var id = 0;
        var dto = new ActionCreateDto(id);
        var permitService = Substitute.For<IPermitService>();
        permitService.FindAsync(id).Returns((PermitViewDto?)null);
        var page = PageModelHelpers.BuildDetailsPageModel(permitService);

        // Act
        var result = await page.OnPostNewActionAsync(id, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_MismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var id = 0;
        var dto = new ActionCreateDto(id);
        var page = PageModelHelpers.BuildDetailsPageModel(Substitute.For<IPermitService>());

        // Act
        var result = await page.OnPostNewActionAsync(999, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
