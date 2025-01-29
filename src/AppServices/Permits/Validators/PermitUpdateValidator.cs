using FluentValidation;
using SWGW.AppServices.Permits.CommandDto;

namespace SWGW.AppServices.Permits.Validators;

public class PermitUpdateValidator : AbstractValidator<PermitUpdateDto>
{
    public PermitUpdateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
