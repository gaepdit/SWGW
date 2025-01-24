using FluentValidation;
using SWGW.AppServices.Perimits.CommandDto;

namespace SWGW.AppServices.Perimits.Validators;

public class PermitCreateValidator : AbstractValidator<PermitCreateDto>
{
    public PermitCreateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
