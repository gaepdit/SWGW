namespace SWGW.AppServices.Perimits.CommandDto;

public interface IPermitCommandDto
{
    public Guid ActionTypeId { get; }
    public string Notes { get; }
}
