using SWGW.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace SWGW.AppServices.Permits.CommandDto;

public record PermitUpdateDto : IPermitCommandDto
{
    // Authorization handler assist properties
    public bool Closed { get; init; }
    public bool IsDeleted { get; init; }

    // Data
    [Display(Name = "Action Type")]
    public Guid ActionTypeId { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string Notes { get; init; } = string.Empty;

    // Meta-data

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date received")]
    public DateOnly ReceivedDate { get; init; }

    [Required]
    [DataType(DataType.Time)]
    [Display(Name = "Time received")]
    public TimeOnly ReceivedTime { get; init; }

    [Required]
    [Display(Name = "Received by")]
    public string? ReceivedById { get; init; }

    // Caller

    [StringLength(150)]
    [Display(Name = "Name")]
    public string? CallerName { get; init; }

    [StringLength(150)]
    [Display(Name = "Represents")]
    public string? CallerRepresents { get; init; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string? CallerEmail { get; init; }

    [Display(Name = "Primary phone")]
    public PhoneNumber? CallerPhoneNumber { get; init; }

    [Display(Name = "Secondary phone")]
    public PhoneNumber? CallerSecondaryPhoneNumber { get; init; }

    [Display(Name = "Other phone")]
    public PhoneNumber? CallerTertiaryPhoneNumber { get; init; }

    [Display(Name = "Caller address")]
    public IncompleteAddress CallerAddress { get; init; } = new();

    
}
