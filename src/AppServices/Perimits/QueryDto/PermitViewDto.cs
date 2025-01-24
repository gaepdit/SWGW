using SWGW.AppServices.PermitActions.Dto;
using SWGW.AppServices.Staff.Dto;
using SWGW.Domain.Entities.Perimits;
using System.ComponentModel.DataAnnotations;

namespace SWGW.AppServices.Perimits.QueryDto;

public record PermitViewDto
{
    public Guid Id { get; init; }

    public PermitStatus Status { get; init; }

    [Display(Name = "Date Received")]
    public DateTimeOffset ReceivedDate { get; init; }

    [Display(Name = "Received By")]
    public StaffViewDto? ReceivedBy { get; init; }

    [Display(Name = "Action Type")]
    public string? ActionTypeName { get; init; }

    public string Notes { get; init; } = string.Empty;

    // Properties: Review/Closure

    [Display(Name = "Permit Closed")]
    public bool Closed { get; init; }

    [Display(Name = "Date Closed")]
    public DateTimeOffset? ClosedDate { get; init; }

    [Display(Name = "Closed By")]
    public StaffViewDto? ClosedBy { get; init; }

    [Display(Name = "Closure Comments")]
    public string? ClosedComments { get; init; }

    // Properties: Deletion

    [Display(Name = "Deleted?")]
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Comments")]
    public string? DeleteComments { get; init; }

    // === Lists ===

    [UsedImplicitly]
    public List<ActionViewDto> PermitActions { get; } = [];
}
