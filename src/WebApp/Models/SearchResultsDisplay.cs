using GaEpd.AppLibrary.Pagination;
using SWGW.AppServices.Permits.QueryDto;

namespace SWGW.WebApp.Models;

public record SearchResultsDisplay(
    IBasicSearchDisplay Spec,
    IPaginatedResult<PermitSearchResultDto> SearchResults,
    PaginationNavModel Pagination,
    bool IsPublic)
{
    public string SortByName => Spec.Sort.ToString();
}
