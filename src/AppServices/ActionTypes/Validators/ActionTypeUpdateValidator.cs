﻿using FluentValidation;
using SWGW.AppServices.ActionTypes;
using SWGW.Domain;
using SWGW.Domain.Entities.ActionTypes;

namespace SWGW.AppServices.EntryTypes.Validators;

public class ActionTypeUpdateValidator : AbstractValidator<ActionTypeUpdateDto>
{
    private readonly IActionTypeRepository _repository;

    public ActionTypeUpdateValidator(IActionTypeRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (_, name, context, token) =>
                await NotDuplicateName(name, context, token).ConfigureAwait(false))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, IValidationContext context,
        CancellationToken token = default)
    {
        var item = await _repository.FindByNameAsync(name, token).ConfigureAwait(false);
        return item is null || item.Id == (Guid)context.RootContextData["Id"];
    }
}
