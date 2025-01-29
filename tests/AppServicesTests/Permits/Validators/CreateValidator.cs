using FluentValidation.TestHelper;
using SWGW.AppServices.Permits.CommandDto;
using SWGW.AppServices.Permits.Validators;
using SWGW.TestData.Constants;

namespace AppServicesTests.Permits.Validators;

public class CreateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new PermitCreateDto
        {
            ActionTypeId = Guid.NewGuid(),
            Notes = TextData.Paragraph,
        };

        var validator = new PermitCreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.ShouldNotHaveValidationErrorFor(dto => dto.ActionTypeId);
        result.ShouldNotHaveValidationErrorFor(dto => dto.Notes);
    }

    [Test]
    public async Task MissingCurrentOffice_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitCreateDto
        {
            ActionTypeId = Guid.NewGuid(),
            Notes = string.Empty,
        };

        var validator = new PermitCreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Notes);
    }
}
