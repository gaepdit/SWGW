using AutoMapper;
using SWGW.AppServices.ServiceBase;
using SWGW.AppServices.UserServices;
using SWGW.Domain.Entities.EntryTypes;

namespace SWGW.AppServices.EntryTypes;

public sealed class EntryTypeService(
    IMapper mapper,
    IEntryTypeRepository repository,
    IEntryTypeManager manager,
    IUserService userService)
    : MaintenanceItemService<EntryType, EntryTypeViewDto, EntryTypeUpdateDto>
        (mapper, repository, manager, userService),
        IEntryTypeService;
