using FluentValidation;
using SWGW.AppServices.WorkEntries.CommandDto;

namespace SWGW.AppServices.WorkEntries.Validators;

public class WorkEntryCreateValidator : AbstractValidator<WorkEntryCreateDto>
{
    public WorkEntryCreateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
