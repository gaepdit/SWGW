﻿using SWGW.AppServices.EntryActions;
using SWGW.AppServices.EntryActions.Dto;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.WorkEntries;
using SWGW.AppServices.WorkEntries.Permissions;
using SWGW.AppServices.WorkEntries.QueryDto;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.PageModelHelpers;

namespace SWGW.WebApp.Pages.Staff.EntryAction;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class EditActionModel(
    IEntryActionService actionService,
    IWorkEntryService workEntryService,
    IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public Guid EntryActionId { get; set; }

    [BindProperty]
    public EntryActionUpdateDto UpdateDto { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public WorkEntryViewDto WorkEntryView { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("Index");

        var item = await actionService.FindForUpdateAsync(actionId.Value);
        if (item is null) return NotFound();

        var workEntryView = await workEntryService.FindAsync(item.WorkEntryId);
        if (workEntryView is null) return NotFound();

        if (!await UserCanEditActionItemsAsync(workEntryView)) return Forbid();

        UpdateDto = item;
        EntryActionId = actionId.Value;
        WorkEntryView = workEntryView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var original = await actionService.FindAsync(EntryActionId);
        if (original is null || original.IsDeleted) return BadRequest();

        var workEntryView = await workEntryService.FindAsync(original.WorkEntryId);
        if (workEntryView is null || !await UserCanEditActionItemsAsync(workEntryView))
            return BadRequest();

        if (!ModelState.IsValid)
        {
            WorkEntryView = workEntryView;
            return Page();
        }

        await actionService.UpdateAsync(EntryActionId, UpdateDto);

        HighlightId = EntryActionId;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Work Entry Action successfully updated.");
        return RedirectToPage("../WorkEntries/Details", pageHandler: null, routeValues: new { workEntryView.Id },
            fragment: HighlightId.ToString());
    }

    private Task<bool> UserCanEditActionItemsAsync(WorkEntryViewDto item) =>
        authorization.Succeeded(User, item, WorkEntryOperation.EditWorkEntry);
}
