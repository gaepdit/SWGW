using ClosedXML.Attributes;
using GaEpd.AppLibrary.Extensions;
using SWGW.Domain.Entities.Permits;

namespace SWGW.AppServices.DataExport;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record SearchResultsExportDto
{
    public SearchResultsExportDto(Permit permit)
    {
        PermitId = permit.Id;
        ReceivedDate = permit.ReceivedDate;
        ReceivedByName = permit.ReceivedBy?.SortableFullName;
        Status = permit.Status.GetDisplayName();
        EntryType = permit.ActionType?.Name;
        DateClosed = permit.ClosedDate;
        Notes = permit.Notes;
        Deleted = permit.IsDeleted ? "Deleted" : "No";
    }

    [XLColumn(Header = "Permit ID")]
    public Guid PermitId { get; init; }

    [XLColumn(Header = "Date Received")]
    public DateTimeOffset ReceivedDate { get; init; }

    [XLColumn(Header = "Received By")]
    public string? ReceivedByName { get; init; }

    [XLColumn(Header = "Status")]
    public string Status { get; init; }

    [XLColumn(Header = "Entry Type")]
    public string? EntryType { get; init; }

    [XLColumn(Header = "Date Closed")]
    public DateTimeOffset? DateClosed { get; init; }

    [XLColumn(Header = "Notes")]
    public string? Notes { get; init; }

    [XLColumn(Header = "Deleted?")]
    public string Deleted { get; init; }
}
