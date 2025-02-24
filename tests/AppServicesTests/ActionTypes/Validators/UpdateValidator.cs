﻿using FluentValidation;
using FluentValidation.TestHelper;
using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.EntryTypes;
using SWGW.AppServices.EntryTypes.Validators;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.TestData.Constants;

namespace AppServicesTests.EntryTypes.Validators;

public class UpdateValidator
{
    private static ValidationContext<ActionTypeUpdateDto> GetContext(ActionTypeUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = Guid.Empty } };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((ActionType?)null);
        var model = new ActionTypeUpdateDto(TextData.ValidName, true);

        var result = await new ActionTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ActionType(Guid.NewGuid(), TextData.ValidName));
        var model = new ActionTypeUpdateDto(TextData.ValidName, true);

        var result = await new ActionTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ActionType(Guid.Empty, TextData.ValidName));
        var model = new ActionTypeUpdateDto(TextData.ValidName, true);

        var result = await new ActionTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IActionTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((ActionType?)null);
        var model = new ActionTypeUpdateDto(TextData.ShortName, true);

        var result = await new ActionTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
