using SWGW.AppServices.PermitActions;
using SWGW.AppServices.PermitActions.Dto;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Permits;
using SWGW.AppServices.Permits.Permissions;
using SWGW.AppServices.Permits.QueryDto;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.PageModelHelpers;


namespace SWGW.WebApp.Pages.Staff.PermitAction;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class RestoreActionModel(
    IActionService actionService,
    IPermitService permitService,
    IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public Guid PermitActionItemId { get; set; }

    [TempData]
    public Guid HighlightId { get; set; }

    public ActionViewDto PermitActionViewDto { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("Index");

        var actionItem = await actionService.FindAsync(actionId.Value);
        if (actionItem is null) return NotFound();

        var permitView = await permitService.FindAsync(actionItem.PermitId);
        if (permitView is null) return NotFound();

        if (!await UserCanManageDeletionsAsync(permitView)) return Forbid();

        if (!actionItem.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "Permit Action cannot be restored because it is not deleted.");
            return RedirectToPage("../Permits/Details", routeValues: new { permitView.Id });
        }

        PermitActionViewDto = actionItem;
        PermitActionItemId = actionId.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var originalActionItem = await actionService.FindAsync(PermitActionItemId);
        if (originalActionItem is null || !originalActionItem.IsDeleted) return BadRequest();

        var permitView = await permitService.FindAsync(originalActionItem.PermitId);
        if (permitView is null || !await UserCanManageDeletionsAsync(permitView))
            return BadRequest();

        await actionService.RestoreAsync(PermitActionItemId);
        HighlightId = PermitActionItemId;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Permit Action successfully restored.");
        return RedirectToPage("../Permits/Details", pageHandler: null, routeValues: new { permitView.Id },
            fragment: HighlightId.ToString());
    }

    private Task<bool> UserCanManageDeletionsAsync(PermitViewDto item) =>
        authorization.Succeeded(User, item, PermitOperation.ManageDeletions);
}
