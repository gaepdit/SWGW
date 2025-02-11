using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.Permits;
using SWGW.Domain.Identity;

namespace SWGW.Domain.Entities.PermitActions;

public class PermitAction : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private PermitAction() { }


    internal PermitAction(Guid id, Permit permit) : base(id) => Permit = permit;
  
    // Properties

    public int PermitId { get; init; }
    public Permit Permit { get; private init; } = null!;

    public string PermitActionType  { get; set; }
    public DateOnly ActionDate { get; set; }

    [StringLength(10_000)]
    public string Comments { get; set; } = string.Empty;

    public DateTimeOffset? EnteredDate { get; init; } = DateTimeOffset.Now;
    // Properties: Deletion

    public ApplicationUser? DeletedBy { get; set; }
}
