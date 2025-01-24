using SWGW.Domain.Entities.ActionTypes;
using SWGW.TestData;

namespace SWGW.LocalRepository.Repositories;

public sealed class LocalActionTypeRepository()
    : NamedEntityRepository<ActionType>(ActionTypeData.GetData), IActionTypeRepository;
