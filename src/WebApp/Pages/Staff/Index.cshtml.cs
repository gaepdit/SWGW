using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Permits;
using SWGW.AppServices.Permits.QueryDto;
using SWGW.AppServices.Staff;
using SWGW.Domain.Entities.Permits;

namespace SWGW.WebApp.Pages.Staff;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DashboardIndexModel(IPermitService permitService, IStaffService staffService, IAuthorizationService authorization) : PageModel
{
    public bool IsStaff { get; private set; }
    public DashboardCard OpenPermits { get; private set; } = null!;

    public async Task<PageResult> OnGetAsync(CancellationToken token)
    {
        var user = await staffService.GetCurrentUserAsync();
        IsStaff = await authorization.Succeeded(User, Policies.StaffUser);

        if (!IsStaff) return Page();

        var spec = new PermitSearchDto { Status = SearchPermitStatus.AllActive };
        var paging = new PaginatedRequest(1, 5, SortBy.ReceivedDateDesc.GetDescription());
        OpenPermits = new DashboardCard("Recent Open Permits")
            { Permits = (await permitService.SearchAsync(spec, paging, token)).Items.ToList() };

        return Page();
    }

    public record DashboardCard(string Title)
    {
        public required IReadOnlyCollection<PermitSearchResultDto> Permits { get; init; }
        public string CardId => Title.ToLowerInvariant().Replace(oldChar: ' ', newChar: '-');
    }
}
