using SWGW.Domain.Entities.Permits;
using SWGW.EfRepository.DbContext;
using System.Linq.Expressions;

namespace SWGW.EfRepository.Repositories;

public sealed class PermitRepository(AppDbContext context)
    : BaseRepository<Permit, AppDbContext>(context), IPermitRepository
{
    // Entity Framework will set the ID automatically.
    public int? GetNextId() => null;
    public Task<Permit?> FindIncludeAllAsync(Guid id, bool includeDeletedActions = false,
        CancellationToken token = default) =>
        Context.Set<Permit>().AsNoTracking()
            .Include(entry => entry.PermitActions
                .Where(action => !action.IsDeleted || includeDeletedActions)
                .OrderByDescending(action => action.ActionDate)
                .ThenBy(action => action.Id)
            ).ThenInclude(action => action.DeletedBy)
            .AsSplitQuery()
            .SingleOrDefaultAsync(entry => entry.Id.Equals(id), token);
   

    public Task<Permit?> FindIncludeAllAsync(int id, bool includeDeletedActions = false, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Permit>> GetListWithMostRecentActionAsync(Expression<Func<Permit, bool>> predicate, string sorting = "", CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
