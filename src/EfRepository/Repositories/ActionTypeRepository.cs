using SWGW.Domain.Entities.ActionTypes;
using SWGW.EfRepository.DbContext;

namespace SWGW.EfRepository.Repositories;

public sealed class ActionTypeRepository(AppDbContext dbContext) :
    NamedEntityRepository<ActionType, AppDbContext>(dbContext), IActionTypeRepository;
