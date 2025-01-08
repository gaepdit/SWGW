using GaEpd.AppLibrary.ListItems;
using SWGW.AppServices.ServiceBase;

namespace SWGW.AppServices.Offices;

public interface IOfficeService : IMaintenanceItemService<OfficeViewDto, OfficeUpdateDto>
{
    Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default);
}
