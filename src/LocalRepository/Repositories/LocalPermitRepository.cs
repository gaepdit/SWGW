using SWGW.Domain.Entities.PermitActions;
using SWGW.Domain.Entities.Permits;
using SWGW.Domain.Entities.Attachments;
using SWGW.TestData;
using System.Linq.Expressions;
using System.Linq;
using GaEpd.AppLibrary.Pagination;

namespace SWGW.LocalRepository.Repositories;

public sealed class LocalPermitRepository(
    IAttachmentRepository attachmentRepository, 
    IActionRepository permitActionRepository)
    : BaseRepository<Permit,int>(PermitData.GetPermits), IPermitRepository
{
    public async Task<Permit?> FindIncludeAllAsync(int id, bool includeDeletedActions = false,
        CancellationToken token = default) =>
        await GetPermitDetailsAsync(await FindAsync(id, token).ConfigureAwait(false), includeDeletedActions, token)
            .ConfigureAwait(false);

    private async Task<Permit?> GetPermitDetailsAsync(Permit? permit, bool includeDeletedActions,
        CancellationToken token)
    {
        if (permit is null) return null;

        permit.Attachments.Clear();
        permit.Attachments.AddRange((await attachmentRepository.GetListAsync(attachment =>
                    attachment.Permit.Id == permit.Id && !attachment.IsDeleted, token)
                .ConfigureAwait(false))
            .OrderBy(attachment => attachment.UploadedDate)
            .ThenBy(attachment => attachment.FileName)
            .ThenBy(attachment => attachment.Id));

        permit.PermitActions.Clear();
        permit.PermitActions.AddRange((await permitActionRepository
                .GetListAsync(action => action.Permit.Id == permit.Id &&
                                        (!action.IsDeleted || includeDeletedActions), token)
                .ConfigureAwait(false))
            .OrderByDescending(action => action.ActionDate)
             .ThenByDescending(action => action.EnteredDate)
            .ThenBy(action => action.Id));

        return permit;
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

}
