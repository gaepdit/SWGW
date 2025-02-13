using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace SWGW.AppServices.Permits.Permissions;

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

    public static readonly PermitOperation Accept = new(nameof(Accept));
    public static readonly PermitOperation Assign = new(nameof(Assign));
    public static readonly PermitOperation EditActions = new(nameof(EditActions));
    public static readonly PermitOperation EditAttachments = new(nameof(EditAttachments));
    public static readonly PermitOperation EditDetails = new(nameof(EditDetails));
    public static readonly PermitOperation Reassign = new(nameof(Reassign));
    public static readonly PermitOperation Reopen = new(nameof(Reopen));
    public static readonly PermitOperation RequestReview = new(nameof(RequestReview));
    public static readonly PermitOperation Review = new(nameof(Review));
    public static readonly PermitOperation ViewAsOwner = new(nameof(ViewAsOwner));

}
