namespace SWGW.WebApp.Pages.Admin.Maintenance;

public class MaintenanceOption
{
    public string SingularName { get; private init; } = string.Empty;
    public string PluralName { get; private init; } = string.Empty;
    public bool StartsWithVowelSound { get; private init; }

    private MaintenanceOption() { }

    public static MaintenanceOption ActionType { get; } =
        new() { SingularName = "Action Type", PluralName = "Action Types", StartsWithVowelSound = false };

    public static MaintenanceOption Office { get; } =
        new() { SingularName = "Office", PluralName = "Offices", StartsWithVowelSound = true };
}
