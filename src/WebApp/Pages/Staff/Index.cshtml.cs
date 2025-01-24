using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.Perimits;
using SWGW.AppServices.Perimits.QueryDto;
using SWGW.Domain.Entities.Perimits;

namespace SWGW.WebApp.Pages.Staff;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DashboardIndexModel(IPermitService permitService, IAuthorizationService authorization) : PageModel
{
    public bool IsStaff { get; private set; }
    public DashboardCard OpenPermits { get; private set; } = null!;

    public async Task<PageResult> OnGetAsync(CancellationToken token)
    {
        IsStaff = await authorization.Succeeded(User, Policies.StaffUser);

        if (!IsStaff) return Page();

        var spec = new PermitSearchDto { Status = PermitStatus.Open };
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
