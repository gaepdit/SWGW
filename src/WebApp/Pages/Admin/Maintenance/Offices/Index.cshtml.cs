﻿using SWGW.AppServices.Offices;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;

namespace SWGW.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel : PageModel
{
    public IReadOnlyList<OfficeViewDto> Items { get; private set; } = null!;
    public static MaintenanceOption ThisOption => MaintenanceOption.Office;
    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync(
        [FromServices] IOfficeService service,
        [FromServices] IAuthorizationService authorization)
    {
        Items = await service.GetListAsync();
        IsSiteMaintainer = await authorization.Succeeded(User, Policies.SiteMaintainer);
    }
}
