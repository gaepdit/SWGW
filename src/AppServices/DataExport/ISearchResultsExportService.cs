using SWGW.AppServices.Permits.QueryDto;

namespace SWGW.AppServices.DataExport;

public interface ISearchResultsExportService : IDisposable, IAsyncDisposable
{
    Task<int> CountAsync(PermitSearchDto spec, CancellationToken token);

    Task<IReadOnlyList<SearchResultsExportDto>> ExportSearchResultsAsync(PermitSearchDto spec,
        CancellationToken token);
}
