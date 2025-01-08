using SWGW.Domain.Entities.EntryTypes;
using SWGW.EfRepository.DbContext;

namespace SWGW.EfRepository.Repositories;

public sealed class EntryTypeRepository(AppDbContext dbContext) :
    NamedEntityRepository<EntryType, AppDbContext>(dbContext), IEntryTypeRepository;
