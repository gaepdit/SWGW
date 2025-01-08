using SWGW.Domain.Entities.EntryTypes;
using SWGW.TestData;

namespace SWGW.LocalRepository.Repositories;

public sealed class LocalEntryTypeRepository()
    : NamedEntityRepository<EntryType>(EntryTypeData.GetData), IEntryTypeRepository;
