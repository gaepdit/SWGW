using System.ComponentModel.DataAnnotations;

namespace SWGW.AppServices.Perimits.CommandDto;

public record PermitCreateDto : IPermitCommandDto
{
    [Display(Name = "Action Type")]
    public Guid ActionTypeId { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string Notes { get; init; } = string.Empty;
    
}
