namespace SWGW.Domain.Identity;

/// <summary>
/// User Roles available to the application for authorization.
/// </summary>
public static class RoleName
{
    // These are the strings that are stored in the database. Avoid modifying these once set!

    public const string ProgramManager = nameof(ProgramManager);
    public const string UnitManager = nameof(UnitManager);
    public const string Engineer = nameof(Engineer);
    public const string Geologist = nameof(Geologist);
    public const string SiteMaintenance = nameof(SiteMaintenance);
    public const string Staff = nameof(Staff);
    public const string UserAdmin = nameof(UserAdmin);
    public const string SuperUserAdmin = nameof(SuperUserAdmin);
}

/// <summary>
/// Class for listing and describing the application roles for use in the UI, etc.
/// </summary>
public class AppRole
{
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }

    private AppRole(string name, string displayName, string description)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
        AllRoles.Add(name, this);
    }

    /// <summary>
    /// A Dictionary of all roles used by the app. The Dictionary key is a string containing 
    /// the <see cref="Microsoft.AspNetCore.Identity.IdentityRole.Name"/> of the role.
    /// (This declaration must appear before the list of static instance types.)
    /// </summary>
    public static Dictionary<string, AppRole> AllRoles { get; } = new();

    /// <summary>
    /// Converts a list of role strings to a list of <see cref="AppRole"/> objects.
    /// </summary>
    /// <param name="roles">A list of role strings.</param>
    /// <returns>A list of AppRoles.</returns>
    public static IEnumerable<AppRole> RolesAsAppRoles(IEnumerable<string> roles)
    {
        var appRoles = new List<AppRole>();

        foreach (var role in roles)
            if (AllRoles.TryGetValue(role, out var appRole))
                appRoles.Add(appRole);

        return appRoles;
    }

    // These static Role objects are used for displaying role information in the UI.

    [UsedImplicitly]
    public static AppRole ProgramManagerRole { get; } = new(
        RoleName.ProgramManager, "Program Manager",
        "Can review all data for surface water and groundwater but not manipulate the data."
    );

    [UsedImplicitly]
    public static AppRole UnitManagerRole { get; } = new(
        RoleName.UnitManager, "Unit Manager",
        "Can have an admin role to be able to add/remove users and have access to all database data (both surface water and groundwater). Manager should be able to manipulate data, create tables from data, as well as view data."
     );

    [UsedImplicitly]
    public static AppRole EngineerRole { get; } = new(
        RoleName.Engineer, "Engineer",
        "Receives applications and conducts reviews of surface water applications. Can develops permits for systems based on applications submitted. Engineer should have access to all surface water data within the database and should only be able to view groundwater data."
     );

    [UsedImplicitly]
    public static AppRole GeologistRole { get; } = new(
        RoleName.Geologist, "Geologist",
        "Receives applications and inputs compliance data into database. Geologist should be able to access all groundwater data but only view surface water data."
    );

    [UsedImplicitly]
    public static AppRole SiteMaintenanceRole { get; } = new(
        RoleName.SiteMaintenance, "Site Maintenance",
        "Can update values in lookup tables (drop-down lists)."
    );

    [UsedImplicitly]
    public static AppRole StaffRole { get; } = new(
        RoleName.Staff, "Staff",
        "Can create new Permits and work with permits assigned to them. Can transfer their permits to other users."
    );

    [UsedImplicitly]
    public static AppRole UserAdminRole { get; } = new(
        RoleName.UserAdmin, "User Account Admin",
        "Can edit all users and roles, excluding the Unit Manager role."
    );

    [UsedImplicitly]
    public static AppRole SuperUserAdminRole { get; } = new(
        RoleName.SuperUserAdmin, "Super-User Account Admin",
        "Can edit all users and roles, including the Unit Manager and Program Manager role."
 );
}
