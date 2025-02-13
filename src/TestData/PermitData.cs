using SWGW.Domain.Entities.Permits;
using SWGW.TestData.Constants;
using SWGW.TestData.Identity;

namespace SWGW.TestData;

internal static class PermitData
{
    private static IEnumerable<Permit> PermitSeedItems => new List<Permit>
    {
        new(1) // 0
        {
            Closed = true,
            Status = PermitStatus.Void,
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            ActionType = ActionTypeData.GetData.ElementAt(0),
            Notes = TextData.Paragraph,
        },
        new(2) // 1
        {
            Closed = false,
            Status = PermitStatus.Active,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            ReceivedDate = DateTimeOffset.Now.AddMinutes(30),
            ActionType = ActionTypeData.GetData.ElementAt(0),
            Notes = TextData.MultipleParagraphs,
        },
        new(3) // 2
        {
            Closed = true,
            Status = PermitStatus.Void,
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            ActionType = ActionTypeData.GetData.ElementAt(1),
        },
        new(4) // 3
        {
            Notes = "Deleted Permit",
            Closed = true,
            Status = PermitStatus.Void,
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            DeleteComments = TextData.Paragraph,
            ActionType = ActionTypeData.GetData.ElementAt(2),
        },
        new(5) // 4
        {
            Closed = false,
            Status = PermitStatus.Active,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            ActionType = null,
        },
        new(6) // 5
        {
            Closed = false,
            Status = PermitStatus.Active,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            ActionType = ActionTypeData.GetData.ElementAt(3),
        },
        new(7) // 6
        {
            Notes = "Open Permit assigned to inactive user.",
            Closed = false,
            Status = PermitStatus.Active,
            ReceivedBy = UserData.GetUsers.ElementAt(2),
            ActionType = ActionTypeData.GetData.ElementAt(0),
        },
    };

    private static IEnumerable<Permit>? _permits;

    public static IEnumerable<Permit> GetPermits
    {
        get
        {
            if (_permits is not null) return _permits;

            _permits = PermitSeedItems.ToList();
            _permits.ElementAt(3).SetDeleted("00000000-0000-0000-0000-000000000001");

            return _permits;
        }
    }

    public static void ClearData() => _permits = null;
}
