using FluentValidation;
using SWGW.AppServices.Perimits.CommandDto;

namespace SWGW.AppServices.Perimits.Validators;

public class PermitUpdateValidator : AbstractValidator<PermitUpdateDto>
{
    public PermitUpdateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
