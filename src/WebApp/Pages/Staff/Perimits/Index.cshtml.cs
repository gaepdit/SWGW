using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.Offices;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Staff;
using SWGW.AppServices.Perimits;
using SWGW.AppServices.Perimits.QueryDto;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.Constants;


namespace SWGW.WebApp.Pages.Staff.Perimits;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel(
    IPermitService permitService,
    IStaffService staff,
    IActionTypeService actionTypeService,
    IOfficeService offices,
    IAuthorizationService authorization)
    : PageModel
{
    public PermitSearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public bool CanViewDeletedPermits { get; private set; }
    public IPaginatedResult<PermitSearchResultDto> SearchResults { get; private set; } = null!;
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());
    public SearchResultsDisplay ResultsDisplay => new(Spec, SearchResults, PaginationNav, IsPublic: false);

    public SelectList ReceivedBySelectList { get; private set; } = null!;
    public SelectList ActionTypesSelectList { get; private set; } = null!;
    public SelectList OfficesSelectList { get; set; } = null!;

    public async Task OnGetAsync()
    {
        Spec = new PermitSearchDto();
        CanViewDeletedPermits = await authorization.Succeeded(User, Policies.Manager);
        await PopulateSelectListsAsync();
    }

    public async Task<IActionResult> OnGetSearchAsync(PermitSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();
        CanViewDeletedPermits = await authorization.Succeeded(User, Policies.Manager);
        await PopulateSelectListsAsync();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, Spec.Sort.GetDescription());
        SearchResults = await permitService.SearchAsync(Spec, paging);
        ShowResults = true;
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        ReceivedBySelectList = (await staff.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        ActionTypesSelectList = (await actionTypeService.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        OfficesSelectList = (await offices.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
    }
}
