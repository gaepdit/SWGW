namespace SWGW.AppServices.Perimits.QueryDto;

public interface IBasicSearchDisplay
{
    SortBy Sort { get; }
    IDictionary<string, string?> AsRouteValues();
}
