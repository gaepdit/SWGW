using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Permits;
using SWGW.AppServices.Permits.CommandDto;
using SWGW.AppServices.Permits.Permissions;
using SWGW.AppServices.Permits.QueryDto;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.PageModelHelpers;

namespace SWGW.WebApp.Pages.Staff.Permits;

[Authorize(Policy = nameof(Policies.Manager))]
public class RestoreModel(IPermitService permitService, IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public PermitChangeStatusDto PermitDto { get; set; } = null!;

    public PermitViewDto PermitView { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("Index");

        var permitView = await permitService.FindAsync(id.Value);
        if (permitView is null) return NotFound();

        if (!await UserCanManageDeletionsAsync(permitView)) return Forbid();

        if (!permitView.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "Permit cannot be restored because it is not deleted.");
            return RedirectToPage("Details", routeValues: new { id });
        }

        PermitDto = new PermitChangeStatusDto(id.Value);
        PermitView = permitView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var permitView = await permitService.FindAsync(PermitDto.PermitId);
        if (permitView is null || !permitView.IsDeleted || !await UserCanManageDeletionsAsync(permitView))
            return BadRequest();

        await permitService.RestoreAsync(PermitDto);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Permit successfully restored.");

        return RedirectToPage("Details", new { id = PermitDto.PermitId });
    }

    private Task<bool> UserCanManageDeletionsAsync(PermitViewDto item) =>
        authorization.Succeeded(User, item, PermitOperation.ManageDeletions);
}
