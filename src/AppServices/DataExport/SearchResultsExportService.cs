using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.UserServices;
using SWGW.AppServices.Perimits;
using SWGW.AppServices.Perimits.QueryDto;
using SWGW.Domain.Entities.Permits;
using SWGW.AppServices.Perimits;

namespace SWGW.AppServices.DataExport;

public sealed class SearchResultsExportService(
    IPermitRepository permitRepository,
    IUserService userService,
    IAuthorizationService authorization)
    : ISearchResultsExportService
{
    public async Task<int> CountAsync(PermitSearchDto spec, CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.Manager).ConfigureAwait(false))
            spec.DeletedStatus = null;

        return await permitRepository.CountAsync(PermitFilters.SearchPredicate(spec), token)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<SearchResultsExportDto>> ExportSearchResultsAsync(PermitSearchDto spec,
        CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.Manager).ConfigureAwait(false))
            spec.DeletedStatus = null;

        return (await permitRepository.GetListAsync(PermitFilters.SearchPredicate(spec), token)
                .ConfigureAwait(false))
            .Select(entry => new SearchResultsExportDto(entry)).ToList();
    }

    #region IDisposable,  IAsyncDisposable

    void IDisposable.Dispose() => permitRepository.Dispose();
    async ValueTask IAsyncDisposable.DisposeAsync() => await permitRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
