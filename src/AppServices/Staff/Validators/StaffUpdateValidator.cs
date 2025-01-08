using FluentValidation;
using SWGW.AppServices.Staff.Dto;
using SWGW.Domain.Identity;

namespace SWGW.AppServices.Staff.Validators;

[UsedImplicitly]
public class StaffUpdateValidator : AbstractValidator<StaffUpdateDto>
{
    public StaffUpdateValidator()
    {
        RuleFor(dto => dto.PhoneNumber)
            .MaximumLength(ApplicationUser.MaxPhoneLength);
    }
}
