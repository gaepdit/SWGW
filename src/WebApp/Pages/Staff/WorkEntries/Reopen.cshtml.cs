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
public class ReopenModel(IWorkEntryService workEntryService, IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public WorkEntryChangeStatusDto EntryDto { get; set; } = default!;

    public WorkEntryViewDto ItemView { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");

        var workEntryView = await workEntryService.FindAsync(id.Value);
        if (workEntryView is null) return NotFound();

        if (!await UserCanReviewAsync(workEntryView)) return Forbid();

        EntryDto = new WorkEntryChangeStatusDto(id.Value);
        ItemView = workEntryView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var workEntryView = await workEntryService.FindAsync(EntryDto.WorkEntryId);
        if (workEntryView is null || !await UserCanReviewAsync(workEntryView))
            return BadRequest();

        await workEntryService.CloseAsync(EntryDto);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "The Work Entry has been reopened.");

        var notificationResult = await workEntryService.ReopenAsync(EntryDto);
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            "The WorkEntry has been reopened.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { id = EntryDto.WorkEntryId });
    }

    private Task<bool> UserCanReviewAsync(WorkEntryViewDto item) =>
        authorization.Succeeded(User, item, WorkEntryOperation.EditWorkEntry);
}
