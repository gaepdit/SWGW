﻿using GaEpd.AppLibrary.Pagination;
using SWGW.AppServices.Notifications;
using SWGW.AppServices.WorkEntries.CommandDto;
using SWGW.AppServices.WorkEntries.QueryDto;

namespace SWGW.AppServices.WorkEntries;

public interface IWorkEntryService : IDisposable, IAsyncDisposable
{
    Task<WorkEntryViewDto?> FindAsync(Guid id, bool includeDeletedActions = false, CancellationToken token = default);

    Task<WorkEntryUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);

    Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchAsync(WorkEntrySearchDto spec, PaginatedRequest paging,
        CancellationToken token = default);

    Task<WorkEntryCreateResult> CreateAsync(WorkEntryCreateDto resource, CancellationToken token = default);

    Task UpdateAsync(Guid id, WorkEntryUpdateDto resource, CancellationToken token = default);

    Task CloseAsync(WorkEntryChangeStatusDto resource, CancellationToken token = default);

    Task<NotificationResult> ReopenAsync(WorkEntryChangeStatusDto resource, CancellationToken token = default);

    Task DeleteAsync(WorkEntryChangeStatusDto resource, CancellationToken token = default);

    Task RestoreAsync(WorkEntryChangeStatusDto resource, CancellationToken token = default);
}
