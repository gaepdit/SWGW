using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.Staff;
using SWGW.AppServices.Permits;
using SWGW.AppServices.Permits.CommandDto;
using SWGW.WebApp.Pages.Staff.Permits;

namespace WebAppTests.PermitPages;

[TestFixture]
public class EditPageTests
{
    private IPermitService _permitService = null!;
    private IStaffService _staffService = null!;
    private IActionTypeService _permitTypeService = null!;

    [SetUp]
    public void Setup()
    {
        _permitService = Substitute.For<IPermitService>();
        _staffService = Substitute.For<IStaffService>();
        _permitTypeService = Substitute.For<IActionTypeService>();
        _permitTypeService.GetAsListItemsAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(new List<ListItem>());
        _staffService.GetAsListItemsAsync(Arg.Any<bool>()).Returns(new List<ListItem<string>>());
    }

    [TearDown]
    public void Teardown()
    {
        _permitService.Dispose();
        _staffService.Dispose();
        _permitTypeService.Dispose();
    }

    [Test]
    public async Task OnGet_ReturnsPage()
    {
        // Arrange
        var id = 0;
        var dto = new PermitUpdateDto();

        var permitService = Substitute.For<IPermitService>();
        permitService.FindForUpdateAsync(id).Returns(dto);

        var authorization = Substitute.For<IAuthorizationService>();
        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        var page = new EditModel(permitService, _permitTypeService, Substitute.For<IValidator<PermitUpdateDto>>(),
            authorization);

        // Act
        var result = await page.OnGetAsync(id);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.Item.Should().BeOfType<PermitUpdateDto>();
        page.Item.Should().Be(dto);
    }

    [Test]
    public async Task OnPost_ReturnsRedirectResultWhenModelIsValid()
    {
        // Arrange
        var validator = Substitute.For<IValidator<PermitUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_permitService, _permitTypeService, validator, authorization)
        {
            Id = 1,
            Item = new PermitUpdateDto(),
            TempData = WebAppTestsSetup.PageTempData(),
        };

        _permitService.FindForUpdateAsync(page.Id).Returns(page.Item);

        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());

        validator.ValidateAsync(Arg.Any<PermitUpdateDto>())
            .Returns(new ValidationResult());

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<RedirectToPageResult>();
    }

    [Test]
    public async Task OnPost_ReturnsBadRequestWhenOriginalEntryIsNull()
    {
        // Arrange
        var validator = Substitute.For<IValidator<PermitUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_permitService, _permitTypeService, validator, authorization)
        {
            Id = 2,
            TempData = WebAppTestsSetup.PageTempData(),
        };

        _permitService.FindForUpdateAsync(page.Id).Returns((PermitUpdateDto?)null);

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_ReturnsBadRequestWhenUserCannotEdit()
    {
        // Arrange
        var validator = Substitute.For<IValidator<PermitUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_permitService, _permitTypeService, validator, authorization)
            { Id = 3 };

        _permitService.FindForUpdateAsync(page.Id)
            .Returns(new PermitUpdateDto());
        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Failed());
        validator.ValidateAsync(Arg.Any<PermitUpdateDto>())
            .Returns(new ValidationResult());

        // Act
        var result = await page.OnPostAsync();

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_ReturnsPageResultWhenModelStateIsNotValid()
    {
        // Arrange
        var validator = Substitute.For<IValidator<PermitUpdateDto>>();
        var authorization = Substitute.For<IAuthorizationService>();
        var page = new EditModel(_permitService, _permitTypeService, validator, authorization)
            { Id =4 };

        page.ModelState.AddModelError("test", "test error");

        _permitService.FindForUpdateAsync(page.Id)
            .Returns(new PermitUpdateDto());
        authorization.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object?>(),
                Arg.Any<IEnumerable<IAuthorizationRequirement>>())
            .Returns(AuthorizationResult.Success());
        validator.ValidateAsync(Arg.Any<PermitUpdateDto>())
            .Returns(new ValidationResult());

        // Act
        var result = await page.OnPostAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<PageResult>();
        page.ModelState.IsValid.Should().BeFalse();
    }
}
