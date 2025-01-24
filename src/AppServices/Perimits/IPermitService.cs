using GaEpd.AppLibrary.Pagination;
using SWGW.AppServices.Notifications;
using SWGW.AppServices.Perimits;
using SWGW.AppServices.Perimits.CommandDto;
using SWGW.AppServices.Perimits.QueryDto;

namespace SWGW.AppServices.Perimits;

public interface IPermitService : IDisposable, IAsyncDisposable
{
    Task<PermitViewDto?> FindAsync(Guid id, bool includeDeletedActions = false, CancellationToken token = default);

    Task<PermitUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);

    Task<IPaginatedResult<PermitSearchResultDto>> SearchAsync(PermitSearchDto spec, PaginatedRequest paging,
        CancellationToken token = default);

    Task<PermitCreateResult> CreateAsync(PermitCreateDto resource, CancellationToken token = default);

    Task UpdateAsync(Guid id, PermitUpdateDto resource, CancellationToken token = default);

    Task CloseAsync(PermitChangeStatusDto resource, CancellationToken token = default);

    Task<NotificationResult> ReopenAsync(PermitChangeStatusDto resource, CancellationToken token = default);

    Task DeleteAsync(PermitChangeStatusDto resource, CancellationToken token = default);

    Task RestoreAsync(PermitChangeStatusDto resource, CancellationToken token = default);
}
