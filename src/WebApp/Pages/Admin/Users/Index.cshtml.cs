﻿using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using SWGW.AppServices.Offices;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Staff;
using SWGW.AppServices.Staff.Dto;
using SWGW.Domain.Identity;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.Constants;

namespace SWGW.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class UsersIndexModel(IOfficeService officeService, IStaffService staffService) : PageModel
{
    public StaffSearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<StaffSearchResultDto> SearchResults { get; private set; } = null!;
    public string SortByName => Spec.Sort.ToString();
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());

    public SelectList RolesSelectList { get; private set; } = null!;
    public SelectList OfficesSelectList { get; private set; } = null!;

    public Task OnGetAsync() => PopulateSelectListsAsync();

    public async Task<IActionResult> OnGetSearchAsync(StaffSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();
        await PopulateSelectListsAsync();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, Spec.Sort.GetDescription());
        SearchResults = await staffService.SearchAsync(Spec, paging);
        ShowResults = true;
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        OfficesSelectList = (await officeService.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        RolesSelectList = AppRole.AllRoles
            .Select(pair => new ListItem<string>(pair.Key, pair.Value.DisplayName))
            .ToSelectList();
    }
}
