using FluentValidation;
using SWGW.AppServices.Permits.CommandDto;

namespace SWGW.AppServices.Permits.Validators;

public class PermitCreateValidator : AbstractValidator<PermitCreateDto>
{
    public PermitCreateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
