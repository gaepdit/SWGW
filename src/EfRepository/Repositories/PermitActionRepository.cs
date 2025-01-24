using SWGW.Domain.Entities.PermitActions;
using SWGW.EfRepository.DbContext;

namespace SWGW.EfRepository.Repositories;

public sealed class PermitActionRepository(AppDbContext dbContext)
    : BaseRepository<PermitAction, AppDbContext>(dbContext), IActionRepository;
