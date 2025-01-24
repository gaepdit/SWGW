using SWGW.TestData;
using SWGW.TestData.Identity;

namespace SWGW.EfRepository.DbContext.DevData;

public static class DbSeedDataHelpers
{
    public static void SeedAllData(AppDbContext context)
    {
        SeedOfficeData(context);
        SeedIdentityData(context);
        SeedActionTypeData(context);
        SeedPermitData(context);
    }

    internal static void SeedActionTypeData(AppDbContext context)
    {
        if (context.ActionTypes.Any()) return;
        context.ActionTypes.AddRange(ActionTypeData.GetData);
        context.SaveChanges();
    }

    private static void SeedPermitData(AppDbContext context)
    {
        if (context.Permits.Any()) return;

        context.Database.BeginTransaction();

        context.Permits.AddRange(PermitData.GetPermits);
        context.SaveChanges();

        if (!context.PermitActions.Any())
        {
            context.PermitActions.AddRange(PermitActionData.GetData);
            context.SaveChanges();
        }

        context.Database.CommitTransaction();
    }

    internal static void SeedOfficeData(AppDbContext context)
    {
        if (context.Offices.Any()) return;
        context.Offices.AddRange(OfficeData.GetData);
        context.SaveChanges();
    }

    internal static void SeedIdentityData(AppDbContext context)
    {
        // Seed Users
        var users = UserData.GetUsers.ToList();
        if (!context.Users.Any()) context.Users.AddRange(users);

        // Seed Roles
        var roles = UserData.GetRoles.ToList();
        if (!context.Roles.Any()) context.Roles.AddRange(roles);

        context.SaveChanges();
    }
}
