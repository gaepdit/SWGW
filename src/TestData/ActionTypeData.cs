using SWGW.Domain.Entities.ActionTypes;

namespace SWGW.TestData;

internal static class ActionTypeData
{
    private static IEnumerable<ActionType> EntryTypeSeedItems => new List<ActionType>
    {
        new(new Guid("20000000-0000-0000-0000-000000000000"), "New"), // 1
        new(new Guid("20000000-0000-0000-0000-000000000001"), "Modification"), // 2
        new(new Guid("20000000-0000-0000-0000-000000000002"), "Renewal"), // 3
        new(new Guid("20000000-0000-0000-0000-000000000003"), "OtherChanges") { Active = false }, // 4
    };

    private static IEnumerable<ActionType>? _entryTypes;

    public static IEnumerable<ActionType> GetData
    {
        get
        {
            if (_entryTypes is not null) return _entryTypes;
            _entryTypes = EntryTypeSeedItems;
            return _entryTypes;
        }
    }

    public static void ClearData() => _entryTypes = null;
}
