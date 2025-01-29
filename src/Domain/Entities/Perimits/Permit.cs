using SWGW.Domain.Entities.PermitActions;
using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Identity;
using System.Text.Json.Serialization;
using SWGW.Domain.Entities.ActionTypes;

namespace SWGW.Domain.Entities.Permits;

public class Permit : AuditableSoftDeleteEntity
{

    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Permit() { }

    internal Permit(Guid id) : base(id) { }
  
    // Properties

    // Properties: Status & meta-data

    [StringLength(25)]
    public PermitStatus Status { get; internal set; } = PermitStatus.Open;

    public DateTimeOffset ReceivedDate { get; init; } = DateTimeOffset.Now;
    public ApplicationUser? ReceivedBy { get; init; }

    public ApplicationUser? EnteredBy { get; init; }


    // Properties: Data

    public ActionType? ActionType { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    // Properties: Actions
    public List<PermitAction> PermitActions { get; } = [];

    // Properties: Closure

    public bool Closed { get; internal set; }
    public ApplicationUser? ClosedBy { get; internal set; }
    public DateTimeOffset? ClosedDate { get; internal set; }

    [StringLength(7000)]
    public string? ClosedComments { get; internal set; }

    // Properties: Deletion

    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }
}

// Enums

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PermitStatus
{
    Open,
    Closed,
    Inactive
}
