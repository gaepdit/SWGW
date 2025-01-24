using SWGW.Domain.Entities.PermitActions;
using SWGW.Domain.Entities.Perimits;
using SWGW.TestData;
using System.Linq.Expressions;
using System.Linq;
using GaEpd.AppLibrary.Pagination;

namespace SWGW.LocalRepository.Repositories;

public sealed class LocalPermitRepository(IActionRepository entryActionRepository)
    : BaseRepository<Permit>(PermitData.GetPermits), IPermitRepository
{
    public async Task<Permit?> FindIncludeAllAsync(Guid id, bool includeDeletedActions = false,
        CancellationToken token = default) =>
        await GetPermitDetailsAsync(await FindAsync(id, token).ConfigureAwait(false), includeDeletedActions, token)
            .ConfigureAwait(false);

    private async Task<Permit?> GetPermitDetailsAsync(Permit? workEntry, bool includeDeletedActions,
        CancellationToken token)
    {
        if (workEntry is null) return null;

        workEntry.PermitActions.Clear();
        workEntry.PermitActions.AddRange((await entryActionRepository
                .GetListAsync(action => action.Permit.Id == workEntry.Id &&
                                        (!action.IsDeleted || includeDeletedActions), token)
                .ConfigureAwait(false))
            .OrderByDescending(action => action.ActionDate)
             .ThenByDescending(action => action.EnteredDate)
            .ThenBy(action => action.Id));

        return workEntry;
    }

    public int? GetNextId() => null;
   
    public async Task<Permit?> FindPublicAsync(Expression<Func<Permit, bool>> predicate,
        CancellationToken token = default) =>
        await GetPermitDetailsAsync(await FindAsync(predicate, token).ConfigureAwait(false), false, token)
            .ConfigureAwait(false);

    public Task<IReadOnlyCollection<Permit>> GetListWithMostRecentActionAsync(
        Expression<Func<Permit, bool>> predicate, string sorting = "", CancellationToken token = default)
    {
        var permits = Items.Where(predicate.Compile()).AsQueryable().OrderByIf(sorting);


        return Task.FromResult<IReadOnlyCollection<Permit>>(permits.ToList());
    }
  

    public Task<Permit?> FindIncludeAllAsync(int id, bool includeDeletedActions = false, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
