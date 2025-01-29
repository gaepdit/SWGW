namespace SWGW.AppServices.Permits.CommandDto;

public interface IPermitCommandDto
{
    public Guid ActionTypeId { get; }
    public string Notes { get; }
}
