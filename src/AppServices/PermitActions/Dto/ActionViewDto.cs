using SWGW.AppServices.Staff.Dto;
using System.ComponentModel.DataAnnotations;

namespace SWGW.AppServices.PermitActions.Dto;

public record ActionViewDto
{
    public Guid Id { get; [UsedImplicitly] init; }
    public int PermitId { get; [UsedImplicitly] init; }

    [Display(Name = "Action Date")]
    public DateOnly ActionDate { get; init; }

    public string Comments { get; init; } = string.Empty;

    // Properties: Deletion

    [Display(Name = "Deleted?")]
    public bool IsDeleted { get; init; }

    public string? DeletedById { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; set; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }
}
