using GaEpd.AppLibrary.Pagination;
using SWGW.AppServices.WorkEntries.QueryDto;

namespace SWGW.WebApp.Models;

public record SearchResultsDisplay(
    IBasicSearchDisplay Spec,
    IPaginatedResult<WorkEntrySearchResultDto> SearchResults,
    PaginationNavModel Pagination,
    bool IsPublic)
{
    public string SortByName => Spec.Sort.ToString();
}
