using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.Perimits;
using System.ComponentModel.DataAnnotations;

namespace SWGW.AppServices.Perimits.QueryDto;

public record PermitSearchDto : IBasicSearchDisplay
{
    public SortBy Sort { get; init; } = SortBy.IdAsc;

    [Display(Name = "Permit Status")]
    public PermitStatus? Status { get; init; }

    [Display(Name = "Deletion Status")]
    public SearchDeleteStatus? DeletedStatus { get; set; }

    [Display(Name = "From")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ReceivedFrom { get; init; }

    [Display(Name = "Through")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ReceivedTo { get; init; }

    [Display(Name = "Received By")]
    public string? ReceivedBy { get; init; }

    [Display(Name = "Action Type")]
    public Guid? ActionType { get; init; }

    [Display(Name = "Notes text")]
    public string? Text { get; init; }

    // UI Routing
    public IDictionary<string, string?> AsRouteValues() => new Dictionary<string, string?>
    {
        { nameof(Sort), Sort.ToString() },
        { nameof(Status), Status?.ToString() },
        { nameof(DeletedStatus), DeletedStatus?.ToString() },
        { nameof(ReceivedFrom), ReceivedFrom?.ToString("d") },
        { nameof(ReceivedTo), ReceivedTo?.ToString("d") },
        { nameof(ActionType), ActionType?.ToString() },
        { nameof(Text), Text },
    };

    public PermitSearchDto TrimAll() => this with
    {
        Text = Text?.Trim(),
    };
}
