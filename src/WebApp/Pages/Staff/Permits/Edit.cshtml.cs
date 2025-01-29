using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Permits;
using SWGW.AppServices.Permits.CommandDto;
using SWGW.AppServices.Permits.Permissions;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.PageModelHelpers;


namespace SWGW.WebApp.Pages.Staff.Permits;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class EditModel(
    IPermitService permitService,
    IActionTypeService actionTypeService,
    IValidator<PermitUpdateDto> validator,
    IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public PermitUpdateDto Item { get; set; } = null!;

    public SelectList ActionTypesSelectList { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await permitService.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();
        if (!await UserCanEditAsync(item)) return Forbid();

        Id = id.Value;
        Item = item;

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var original = await permitService.FindForUpdateAsync(Id);
        if (original is null || !await UserCanEditAsync(original)) return BadRequest();

        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await permitService.UpdateAsync(Id, Item);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Permit successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task PopulateSelectListsAsync() =>
        ActionTypesSelectList = (await actionTypeService.GetAsListItemsAsync()).ToSelectList();

    private Task<bool> UserCanEditAsync(PermitUpdateDto item) =>
        authorization.Succeeded(User, item, new PermitUpdateRequirements());
}
