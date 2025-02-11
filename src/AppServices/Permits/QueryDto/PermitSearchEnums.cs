using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWGW.AppServices.Permits.QueryDto;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortBy
{
    [Description("Id")] IdAsc,
    [Description("Id desc")] IdDesc,
    [Description("ReceivedDate, Id")] ReceivedDateAsc,
    [Description("ReceivedDate desc, Id")] ReceivedDateDesc,
    [Description("Status, Id")] StatusAsc,
    [Description("Status desc, Id")] StatusDesc,
}

// The order of the values in this enum is intentional:
// The listed order of the items determines the order they appear in the search form dropdown,
// and the corresponding integer values ensure that previous bookmarks don't break.
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchPermitStatus
{
    [Display(Name = "All Active")] AllActive = 0,
    [Display(Name = "All Void")] AllVoid = 1,
    
}

// "(Any)" (null) = no filtering
// "Closed" = only closed complaints
// "Open" = only open complaints
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PublicSearchStatus
{
    Void = 0,
    Active = 1,
}

// "Not Deleted" is included as an additional Delete Status option in the UI representing the null default state.
// "Deleted" = only deleted entries
// "All" = all entries
// "Not Deleted" (null) = only non-deleted entries
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchDeleteStatus
{
    Deleted = 0,
    All = 1,
}
public enum PermitActionType
{
    [Display(Name = "New")] New = 1,
    [Display(Name = "Modification")] Modification =2,
    [Display(Name = "Renewal")] Renewal = 3,
    [Display(Name = "Other Changes")] OtherChanges = 4,

}