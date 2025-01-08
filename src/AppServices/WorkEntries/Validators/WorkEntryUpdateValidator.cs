using FluentValidation;
using SWGW.AppServices.WorkEntries.CommandDto;

namespace SWGW.AppServices.WorkEntries.Validators;

public class WorkEntryUpdateValidator : AbstractValidator<WorkEntryUpdateDto>
{
    public WorkEntryUpdateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
