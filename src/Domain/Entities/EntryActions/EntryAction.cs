﻿using SWGW.Domain.Entities.WorkEntries;
using SWGW.Domain.Identity;

namespace SWGW.Domain.Entities.EntryActions;

public class EntryAction : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private EntryAction() { }

    internal EntryAction(Guid id, WorkEntry workEntry) : base(id) => WorkEntry = workEntry;

    // Properties

    public WorkEntry WorkEntry { get; private init; } = default!;

    public DateOnly ActionDate { get; set; }

    [StringLength(10_000)]
    public string Comments { get; set; } = string.Empty;

    // Properties: Deletion

    public ApplicationUser? DeletedBy { get; set; }
}
