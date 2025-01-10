using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using SWGW.AppServices.EntryTypes;
using SWGW.AppServices.Offices;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Staff;
using SWGW.AppServices.WorkEntries;
using SWGW.AppServices.WorkEntries.QueryDto;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.Constants;

namespace SWGW.WebApp.Pages.Staff.WorkEntries;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel(
    IWorkEntryService workEntryService,
    IStaffService staff,
    IEntryTypeService entryTypeService,
    IOfficeService offices,
    IAuthorizationService authorization)
    : PageModel
{
    public WorkEntrySearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public bool CanViewDeletedWorkEntries { get; private set; }
    public IPaginatedResult<WorkEntrySearchResultDto> SearchResults { get; private set; } = null!;
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());
    public SearchResultsDisplay ResultsDisplay => new(Spec, SearchResults, PaginationNav, IsPublic: false);

    public SelectList ReceivedBySelectList { get; private set; } = null!;
    public SelectList EntryTypesSelectList { get; private set; } = null!;
    public SelectList OfficesSelectList { get; set; } = null!;

    public async Task OnGetAsync()
    {
        Spec = new WorkEntrySearchDto();
        CanViewDeletedWorkEntries = await authorization.Succeeded(User, Policies.Manager);
        await PopulateSelectListsAsync();
    }

    public async Task<IActionResult> OnGetSearchAsync(WorkEntrySearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();
        CanViewDeletedWorkEntries = await authorization.Succeeded(User, Policies.Manager);
        await PopulateSelectListsAsync();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, Spec.Sort.GetDescription());
        SearchResults = await workEntryService.SearchAsync(Spec, paging);
        ShowResults = true;
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        ReceivedBySelectList = (await staff.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        EntryTypesSelectList = (await entryTypeService.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        OfficesSelectList = (await offices.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
    }
}
