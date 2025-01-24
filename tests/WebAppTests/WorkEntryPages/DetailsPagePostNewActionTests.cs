using SWGW.AppServices.PermitActions.Dto;
using SWGW.AppServices.Perimits;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Perimits.QueryDto;

namespace WebAppTests.PermitPages;

public class DetailsPagePostNewActionTests
{
    [Test]
    public async Task OnPostAsync_NullId_ReturnsRedirectToPageResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new ActionCreateDto(id);
        var page = PageModelHelpers.BuildDetailsPageModel();

        // Act
        var result = await page.OnPostNewActionAsync(null, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_EntryNotFound_ReturnsBadRequestResult()
    {
        // Arrange
        var id = Guid.NewGuid();
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
        var id = Guid.NewGuid();
        var dto = new ActionCreateDto(id);
        var page = PageModelHelpers.BuildDetailsPageModel(Substitute.For<IPermitService>());

        // Act
        var result = await page.OnPostNewActionAsync(Guid.NewGuid(), dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }
}
