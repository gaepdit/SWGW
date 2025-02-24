﻿using GaEpd.AppLibrary.Pagination;
using SWGW.AppServices.Notifications;
using SWGW.AppServices.Permits;
using SWGW.AppServices.Permits.CommandDto;
using SWGW.AppServices.Permits.QueryDto;

namespace SWGW.AppServices.Permits;

public interface IPermitService : IDisposable, IAsyncDisposable
{
    Task<PermitViewDto?> FindAsync(int id, bool includeDeletedActions = false, CancellationToken token = default);

    Task<PermitUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default);

    Task<IPaginatedResult<PermitSearchResultDto>> SearchAsync(PermitSearchDto spec, PaginatedRequest paging,
        CancellationToken token = default);

    Task<PermitCreateResult> CreateAsync(PermitCreateDto resource, CancellationToken token = default);

    Task UpdateAsync(int id, PermitUpdateDto resource, CancellationToken token = default);

    Task CloseAsync(PermitChangeStatusDto resource, CancellationToken token = default);

    Task<NotificationResult> ReopenAsync(PermitChangeStatusDto resource, CancellationToken token = default);

    Task DeleteAsync(PermitChangeStatusDto resource, CancellationToken token = default);

    Task RestoreAsync(PermitChangeStatusDto resource, CancellationToken token = default);
}
