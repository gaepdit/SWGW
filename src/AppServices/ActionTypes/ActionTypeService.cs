using AutoMapper;
using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.ServiceBase;
using SWGW.AppServices.UserServices;
using SWGW.Domain.Entities.ActionTypes;

namespace SWGW.AppServices.ActionTypes;

public sealed class ActionTypeService(
    IMapper mapper,
    IActionTypeRepository repository,
    IActionTypeManager manager,
    IUserService userService)
    : MaintenanceItemService<ActionType, ActionTypeViewDto, ActionTypeUpdateDto>
        (mapper, repository, manager, userService),
        IActionTypeService;
