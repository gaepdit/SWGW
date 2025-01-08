using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.ServiceBase;
using SWGW.AppServices.UserServices;
using SWGW.Domain.Entities.Offices;

namespace SWGW.AppServices.Offices;

public sealed class OfficeService(
    IMapper mapper,
    IOfficeRepository repository,
    IOfficeManager manager,
    IUserService userService,
    IAuthorizationService authorization)
    : MaintenanceItemService<Office, OfficeViewDto, OfficeUpdateDto>
        (mapper, repository, manager, userService),
        IOfficeService
{
    private readonly IUserService _userService = userService;

    public async Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default)
    {
        if (id is null) return Array.Empty<ListItem<string>>();

        var user = _userService.GetCurrentPrincipal();

        if (includeInactive &&
            (user is null || !await authorization.Succeeded(user, Policies.ActiveUser).ConfigureAwait(false)))
            includeInactive = false;

        return (await repository.GetStaffMembersListAsync(id.Value, includeInactive, token).ConfigureAwait(false))
            .Select(staff => new ListItem<string>(staff.Id, staff.SortableNameWithInactive)).ToList();
    }
}
