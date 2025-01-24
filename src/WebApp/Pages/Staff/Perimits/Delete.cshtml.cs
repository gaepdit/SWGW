﻿using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Perimits;
using SWGW.AppServices.Perimits.CommandDto;
using SWGW.AppServices.Perimits.Permissions;
using SWGW.AppServices.Perimits.QueryDto;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.PageModelHelpers;

namespace SWGW.WebApp.Pages.Staff.Perimits;

[Authorize(Policy = nameof(Policies.Manager))]
public class DeleteModel(IPermitService permitService, IAuthorizationService authorization) : PageModel
{
    [BindProperty]
    public PermitChangeStatusDto PermitDto { get; set; } = null!;

    public PermitViewDto PermitView { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");

        var permitView = await permitService.FindAsync(id.Value);
        if (permitView is null) return NotFound();

        if (!await UserCanManageDeletionsAsync(permitView)) return Forbid();

        if (permitView.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Warning,
                "Permitcannot be deleted because it is already deleted.");
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
        if (permitView is null || permitView.IsDeleted || !await UserCanManageDeletionsAsync(permitView))
            return BadRequest();

        await permitService.DeleteAsync(PermitDto);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Permit successfully deleted.");

        return RedirectToPage("Details", new { id = PermitDto.PermitId });
    }

    private Task<bool> UserCanManageDeletionsAsync(PermitViewDto item) =>
        authorization.Succeeded(User, item, PermitOperation.ManageDeletions);
}
