using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Perimits;
using SWGW.AppServices.Perimits.CommandDto;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.PageModelHelpers;

namespace SWGW.WebApp.Pages.Staff.Perimits;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class AddModel(
    IPermitService permitService,
    IActionTypeService actionTypeService,
    IValidator<PermitCreateDto> validator) : PageModel
{
    [BindProperty]
    public PermitCreateDto Item { get; set; } = null!;

    public SelectList ActionTypesSelectList { get; private set; } = null!;

    public async Task OnGetAsync()
    {
        Item = new PermitCreateDto();
        await PopulateSelectListsAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        await validator.ApplyValidationAsync(Item, ModelState);

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        var createResult = await permitService.CreateAsync(Item, token);

        TempData.SetDisplayMessage(
            createResult.HasWarnings ? DisplayMessage.AlertContext.Warning : DisplayMessage.AlertContext.Success,
            "Permit successfully created.", createResult.Warnings);

        return RedirectToPage("Details", new { id = createResult.PermitId });
    }

    private async Task PopulateSelectListsAsync() =>
        ActionTypesSelectList = (await actionTypeService.GetAsListItemsAsync()).ToSelectList();
}
