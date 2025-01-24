using FluentValidation;
using SWGW.AppServices.ActionTypes;
using SWGW.Domain;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.PermitActions;

namespace SWGW.AppServices.ActionTypes.Validators;

public class ActionTypeCreateValidator : AbstractValidator<ActionTypeCreateDto>
{
    private readonly IActionTypeRepository _repository;

    public ActionTypeCreateValidator(IActionTypeRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (_, name, token) => await NotDuplicateName(name, token).ConfigureAwait(false))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, CancellationToken token = default) =>
        await _repository.FindByNameAsync(name, token).ConfigureAwait(false) is null;
}
