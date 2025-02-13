using SWGW.Domain.Entities.PermitActions;
using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Identity;
using System.Text.Json.Serialization;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.Attachments;

namespace SWGW.Domain.Entities.Permits;

public class Permit : AuditableSoftDeleteEntity<int>
{

    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Permit() { }

    internal Permit(int? id)
    {
        if (id is not null) Id = id.Value;
    }
    // Properties

    // Properties: Status & meta-data

    [StringLength(25)]
    public PermitStatus Status { get; internal set; } = PermitStatus.Active;
    public string? PermitType { get; set; }
    public string? PermitNumber { get; set; }
    public string? PermitHolder { get; set; }


    public DateTimeOffset ReceivedDate { get; init; } = DateTimeOffset.Now;
    public ApplicationUser? ReceivedBy { get; init; }

    public ApplicationUser? EnteredBy { get; init; }


    // Properties: Data

    public ActionType? ActionType { get; set; }

    
    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    public PermitActionType PermitActionType { get; internal set; } = PermitActionType.New;

    // Properties: Actions
    public List<PermitAction> PermitActions { get; } = [];

    // Properties: Attachments
    public List<Attachment> Attachments { get; } = [];

    // Properties: Closure

    public bool Closed { get; internal set; }
    public ApplicationUser? ClosedBy { get; internal set; }
    public DateTimeOffset? PermitClosedDate { get; internal set; }
    public bool PermitClosed { get; internal set; }

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
    Active=0,   
    [Display(Name = "Void")]
    Void = 1,
}

public enum PermitActionType
{
    New,
    Modification,
    Renewal,
    OtherChanges,

}

