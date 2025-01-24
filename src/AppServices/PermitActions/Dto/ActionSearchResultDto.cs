using SWGW.AppServices.Staff.Dto;

namespace SWGW.AppServices.PermitActions.Dto;

public record ActionSearchResultDto
{
    public int PermitId { get; init; }
    public string ActionTypeName { get; init; } = string.Empty;
    public DateOnly ActionDate { get; init; }
    public string? Investigator { get; init; }
    public StaffViewDto? EnteredBy { get; init; }
    public string? EnteredByName => EnteredBy?.SortableFullName;
    public string? EnteredByOffice => EnteredBy?.Office?.Name;
    public string? Comments { get; init; }
    public bool IsDeleted { get; init; }
    public bool PermitIsDeleted { get; init; }
}
