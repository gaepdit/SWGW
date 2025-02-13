using System.ComponentModel.DataAnnotations;

namespace SWGW.AppServices.PermitActions.Dto;

public record ActionUpdateDto
{
    public int PermitId { get; init; }

    [Required]
    [Display(Name = "Action Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    public DateOnly? ActionDate { get; init; }

    [Required]
    [DataType(DataType.MultilineText)]
    [StringLength(10_000)]
    public string Comments { get; init; } = string.Empty;
}
