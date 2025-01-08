using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.WorkEntries;
using SWGW.AppServices.WorkEntries.CommandDto;
using SWGW.AppServices.WorkEntries.Permissions;
using SWGW.AppServices.WorkEntries.QueryDto;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.PageModelHelpers;

namespace SWGW.WebApp.Pages.Staff.WorkEntries;

[Authorize(Policy = nameof(Policies.Manager))]
public class RestoreModel(IWorkEntryService workEntryService, IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public WorkEntryChangeStatusDto EntryDto { get; set; } = default!;

    public WorkEntryViewDto ItemView { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");

        var workEntryView = await workEntryService.FindAsync(id.Value);
        if (workEntryView is null) return NotFound();

        if (!await UserCanManageDeletionsAsync(workEntryView)) return Forbid();

        if (!workEntryView.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "WorkEntry cannot be restored because it is not deleted.");
            return RedirectToPage("Details", routeValues: new { id });
        }

        EntryDto = new WorkEntryChangeStatusDto(id.Value);
        ItemView = workEntryView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var workEntryView = await workEntryService.FindAsync(EntryDto.WorkEntryId);
        if (workEntryView is null || !workEntryView.IsDeleted || !await UserCanManageDeletionsAsync(workEntryView))
            return BadRequest();

        await workEntryService.RestoreAsync(EntryDto);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Work Entry successfully restored.");

        return RedirectToPage("Details", new { id = EntryDto.WorkEntryId });
    }

    private Task<bool> UserCanManageDeletionsAsync(WorkEntryViewDto item) =>
        authorization.Succeeded(User, item, WorkEntryOperation.ManageDeletions);
}
