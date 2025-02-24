﻿using SWGW.Domain.Entities.PermitActions;
using SWGW.TestData.Constants;

namespace SWGW.TestData;

internal static class PermitActionData
{
    private static IEnumerable<PermitAction> EntryActionSeedItems => new List<PermitAction>
    {
        new(new Guid("30000000-0000-0000-0000-000000000000"), // 0
            PermitData.GetPermits.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Comments = $"Email: {TextData.ValidEmail} & Phone: {TextData.ValidPhoneNumber}",
            PermitActionType="New"
        },
        new(new Guid("30000000-0000-0000-0000-000000000001"), // 1
            PermitData.GetPermits.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Comments = TextData.EmojiWord,
             PermitActionType="Modification"
        },
        new(new Guid("30000000-0000-0000-0000-000000000002"), // 2
            PermitData.GetPermits.ElementAt(0))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Comments = "Deleted action on closed entry",
             PermitActionType="Modification"
        },
        new(new Guid("30000000-0000-0000-0000-000000000003"), // 3
            PermitData.GetPermits.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-1).Date),
            Comments = "Action on a deleted entry",
             PermitActionType="Renewal"
        },
        new(new Guid("30000000-0000-0000-0000-000000000004"), // 4
            PermitData.GetPermits.ElementAt(5))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
            Comments = "Action on open entry",
             PermitActionType="Renewal"
        },
        new(new Guid("30000000-0000-0000-0000-000000000005"), // 5
            PermitData.GetPermits.ElementAt(5))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Comments = "Deleted action on open entry",
             PermitActionType="OtherChanges"
        },
        new(new Guid("30000000-0000-0000-0000-000000000006"), // 6
            PermitData.GetPermits.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-3).Date),
            Comments = "Deleted action on deleted entry",
             PermitActionType="OtherChanges"
        },
    };

    private static List<PermitAction>? _entryActions;

    public static IEnumerable<PermitAction> GetData
    {
        get
        {
            if (_entryActions is not null) return _entryActions;

            _entryActions = EntryActionSeedItems.ToList();
            _entryActions[2].SetDeleted("00000000-0000-0000-0000-000000000001");
            _entryActions[5].SetDeleted("00000000-0000-0000-0000-000000000001");
            _entryActions[6].SetDeleted("00000000-0000-0000-0000-000000000001");
            return _entryActions;
        }
    }

    public static void ClearData() => _entryActions = null;
}
