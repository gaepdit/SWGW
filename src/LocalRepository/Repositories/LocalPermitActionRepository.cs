using SWGW.Domain.Entities.PermitActions;
using SWGW.TestData;

namespace SWGW.LocalRepository.Repositories;

public sealed class LocalPermitActionRepository() 
    : BaseRepository<PermitAction, Guid>(PermitActionData.GetData), IActionRepository;
