﻿using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Permits;
using SWGW.AppServices.Permits.CommandDto;
using SWGW.AppServices.Permits.Permissions;
using SWGW.AppServices.Permits.QueryDto;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.PageModelHelpers;


namespace SWGW.WebApp.Pages.Staff.Permits;

[Authorize(Policy = nameof(Policies.Manager))]
public class CloseModel(IPermitService permitService, IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public PermitChangeStatusDto PermitDto { get; set; } = null!;

    public PermitViewDto PermitView { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null) return RedirectToPage("Index");

        var permitView = await permitService.FindAsync(id.Value);
        if (permitView is null) return NotFound();

        if (!await UserCanReviewAsync(permitView)) return Forbid();

        PermitDto = new PermitChangeStatusDto(id.Value);
        PermitView = permitView;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var permitView = await permitService.FindAsync(PermitDto.PermitId);
        if (permitView is null || !await UserCanReviewAsync(permitView))
            return BadRequest();

        await permitService.CloseAsync(PermitDto);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "The Permit has been closed.");

        return RedirectToPage("Details", new { id = PermitDto.PermitId });
    }

    private Task<bool> UserCanReviewAsync(PermitViewDto item) =>
        authorization.Succeeded(User, item, PermitOperation.EditPermit);
}
