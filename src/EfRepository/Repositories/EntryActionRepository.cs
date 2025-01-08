using SWGW.Domain.Entities.EntryActions;
using SWGW.EfRepository.DbContext;

namespace SWGW.EfRepository.Repositories;

public sealed class EntryActionRepository(AppDbContext dbContext)
    : BaseRepository<EntryAction, AppDbContext>(dbContext), IEntryActionRepository;
