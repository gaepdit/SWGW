using System.ComponentModel.DataAnnotations;

namespace SWGW.AppServices.Permits.CommandDto;

// Used for closing, reopening, deleting, and restoring Permits.
public record PermitChangeStatusDto(Guid PermitId)
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comments { get; init; } = string.Empty;
}
