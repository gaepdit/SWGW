using SWGW.Domain.Entities.EntryActions;
using SWGW.TestData;

namespace SWGW.LocalRepository.Repositories;

public sealed class LocalEntryActionRepository() 
    : BaseRepository<EntryAction, Guid>(EntryActionData.GetData), IEntryActionRepository;
