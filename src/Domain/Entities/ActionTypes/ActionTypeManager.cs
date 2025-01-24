namespace SWGW.Domain.Entities.ActionTypes;

public class ActionTypeManager(IActionTypeRepository repository)
    : NamedEntityManager<ActionType, IActionTypeRepository>(repository), IActionTypeManager;
