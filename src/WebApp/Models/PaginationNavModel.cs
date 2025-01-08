using GaEpd.AppLibrary.Pagination;

namespace SWGW.WebApp.Models;

public record PaginationNavModel(IPaginatedResult Paging, IDictionary<string, string?> RouteValues);
