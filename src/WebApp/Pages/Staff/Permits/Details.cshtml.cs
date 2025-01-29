using SWGW.AppServices.PermitActions;
using SWGW.AppServices.PermitActions.Dto;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Permits;
using SWGW.AppServices.Permits.Permissions;
using SWGW.AppServices.Permits.QueryDto;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.PageModelHelpers;


namespace SWGW.WebApp.Pages.Staff.Permits;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DetailsModel(
    IPermitService permitService,
    IActionService permitActionService,
    IAuthorizationService authorization) : PageModel
{
    public PermitViewDto ItemView { get; private set; } = null!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; private set; } = new();
    public ActionCreateDto NewAction { get; set; } = null!;

    [TempData]
    public Guid HighlightId { get; set; }

    public bool ViewableActions => ItemView.PermitActions.Exists(action =>
        !action.IsDeleted || UserCan[PermitOperation.ViewDeletedActions]);

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");

        var permitView = await permitService.FindAsync(id.Value, true);
        if (permitView is null) return NotFound();

        await SetPermissionsAsync(permitView);
        if (permitView.IsDeleted && !UserCan[PermitOperation.ManageDeletions]) return NotFound();

        ItemView = permitView;
        NewAction = new ActionCreateDto(permitView.Id);
        return Page();
    }

    /// PostNewAction is used to add a new Action for this Permit.
    public async Task<IActionResult> OnPostNewActionAsync(Guid? id, ActionCreateDto newAction,
        CancellationToken token)
    {
        if (id is null || newAction.PermitId != id) return BadRequest();

        var permitView = await permitService.FindAsync(id.Value, includeDeletedActions: true, token);
        if (permitView is null || permitView.IsDeleted) return BadRequest();

        await SetPermissionsAsync(permitView);
        if (!UserCan[PermitOperation.EditPermit]) return BadRequest();

        if (!ModelState.IsValid)
        {
            ItemView = permitView;
            return Page();
        }

        HighlightId = await permitActionService.CreateAsync(newAction, token);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "New Permit Action successfully added.");
        return RedirectToPage("Details", pageHandler: null, routeValues: new { id }, fragment: HighlightId.ToString());
    }

    private async Task SetPermissionsAsync(PermitViewDto item)
    {
        foreach (var operation in PermitOperation.AllOperations)
            UserCan[operation] = await authorization.Succeeded(User, item, operation);
    }
}
