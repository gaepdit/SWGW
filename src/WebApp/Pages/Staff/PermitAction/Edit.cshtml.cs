using SWGW.AppServices.PermitActions;
using SWGW.AppServices.PermitActions.Dto;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Perimits;
using SWGW.AppServices.Perimits.Permissions;
using SWGW.AppServices.Perimits.QueryDto;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.PageModelHelpers;

namespace SWGW.WebApp.Pages.Staff.PermitAction;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class EditActionModel(
    IActionService actionService,
    IPermitService permitService,
    IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public Guid PermitActionId { get; set; }

    [BindProperty]
    public ActionUpdateDto UpdateDto { get; set; } = null!;

    [TempData]
    public Guid HighlightId { get; set; }

    public PermitViewDto PermitView { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("Index");

        var item = await actionService.FindForUpdateAsync(actionId.Value);
        if (item is null) return NotFound();

        var permitView = await permitService.FindAsync(item.PermitId);
        if (permitView is null) return NotFound();

        if (!await UserCanEditActionItemsAsync(permitView)) return Forbid();

        UpdateDto = item;
        PermitActionId = actionId.Value;
        PermitView = permitView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var original = await actionService.FindAsync(PermitActionId);
        if (original is null || original.IsDeleted) return BadRequest();

        var permitView = await permitService.FindAsync(original.Id);
        if (permitView is null || !await UserCanEditActionItemsAsync(permitView))
            return BadRequest();

        if (!ModelState.IsValid)
        {
            PermitView = permitView;
            return Page();
        }

        await actionService.UpdateAsync(PermitActionId, UpdateDto);

        HighlightId = PermitActionId;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Permit Action successfully updated.");
        return RedirectToPage("../Permits/Details", pageHandler: null, routeValues: new { permitView.Id },
            fragment: HighlightId.ToString());
    }

    private Task<bool> UserCanEditActionItemsAsync(PermitViewDto item) =>
        authorization.Succeeded(User, item, PermitOperation.EditPermit);
}
