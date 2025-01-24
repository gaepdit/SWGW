using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace SWGW.AppServices.Perimits.Permissions;

public class PermitOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private PermitOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<PermitOperation> AllOperations { get; } = [];

    public static readonly PermitOperation EditPermit = new(nameof(EditPermit));
    public static readonly PermitOperation ManageDeletions = new(nameof(ManageDeletions));
    public static readonly PermitOperation ViewDeletedActions = new(nameof(ViewDeletedActions));
}
