using SWGW.Domain.Entities.Permits;
using SWGW.TestData.Constants;
using SWGW.TestData.Identity;

namespace SWGW.TestData;

internal static class PermitData
{
    private static IEnumerable<Permit> PermitSeedItems => new List<Permit>
    {
        new(new Guid("10000000-0000-0000-0000-000000000000")) // 0
        {
            Closed = true,
            Status = PermitStatus.Void,
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            ActionType = ActionTypeData.GetData.ElementAt(0),
            Notes = TextData.Paragraph,
        },
        new(new Guid("10000000-0000-0000-0000-000000000001")) // 1
        {
            Closed = false,
            Status = PermitStatus.Active,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            ReceivedDate = DateTimeOffset.Now.AddMinutes(30),
            ActionType = ActionTypeData.GetData.ElementAt(0),
            Notes = TextData.MultipleParagraphs,
        },
        new(new Guid("10000000-0000-0000-0000-000000000002")) // 2
        {
            Closed = true,
            Status = PermitStatus.Void,
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            ActionType = ActionTypeData.GetData.ElementAt(1),
        },
        new(new Guid("10000000-0000-0000-0000-000000000003")) // 3
        {
            Notes = "Deleted Permit",
            Closed = true,
            Status = PermitStatus.Void,
            ReceivedBy = UserData.GetUsers.ElementAt(0),
            DeleteComments = TextData.Paragraph,
            ActionType = ActionTypeData.GetData.ElementAt(2),
        },
        new(new Guid("10000000-0000-0000-0000-000000000004")) // 4
        {
            Closed = false,
            Status = PermitStatus.Active,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            ActionType = null,
        },
        new(new Guid("10000000-0000-0000-0000-000000000005")) // 5
        {
            Closed = false,
            Status = PermitStatus.Active,
            ReceivedBy = UserData.GetUsers.ElementAt(1),
            ActionType = ActionTypeData.GetData.ElementAt(3),
        },
        new(new Guid("10000000-0000-0000-0000-000000000006")) // 6
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
