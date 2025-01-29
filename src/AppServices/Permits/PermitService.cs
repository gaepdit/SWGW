using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using SWGW.AppServices.Notifications;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.AppServices.UserServices;
using SWGW.AppServices.Perimits.CommandDto;
using SWGW.AppServices.Perimits.QueryDto;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.Permits;
using SWGW.Domain.Identity;
using System.Linq.Expressions;
using SWGW.AppServices.Perimits;


namespace SWGW.AppServices.Perimits;

public sealed class PermitService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IPermitRepository permitRepository,
    IActionTypeRepository actionTypeRepository,
    IPermitManager permitManager,
    INotificationService notificationService,
    IUserService userService,
    IAuthorizationService authorization) : IPermitService
{
    public async Task<PermitViewDto?> FindAsync(Guid id, bool includeDeletedActions = false,
        CancellationToken token = default)
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.Manager).ConfigureAwait(false))
            includeDeletedActions = false;
        var workEntry = await permitRepository.FindIncludeAllAsync(id, includeDeletedActions, token)
            .ConfigureAwait(false);
        return workEntry is null ? null : mapper.Map<PermitViewDto>(workEntry);
    }

    public async Task<PermitUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<PermitUpdateDto>(await permitRepository
            .FindAsync(entry => entry.Id == id && !entry.IsDeleted, token)
            .ConfigureAwait(false));

    public async Task<IPaginatedResult<PermitSearchResultDto>> SearchAsync(PermitSearchDto spec,
        PaginatedRequest paging, CancellationToken token = default)
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.Manager).ConfigureAwait(false))
            spec.DeletedStatus = null;
        return await PerformPagedSearchAsync(paging, PermitFilters.SearchPredicate(spec), token)
            .ConfigureAwait(false);
    }

    private async Task<IPaginatedResult<PermitSearchResultDto>> PerformPagedSearchAsync(PaginatedRequest paging,
        Expression<Func<Permit, bool>> predicate, CancellationToken token)
    {
        var count = await permitRepository.CountAsync(predicate, token).ConfigureAwait(false);
        var items = count > 0
            ? mapper.Map<IEnumerable<PermitSearchResultDto>>(await permitRepository
                .GetPagedListAsync(predicate, paging, token).ConfigureAwait(false))
            : [];
        return new PaginatedResult<PermitSearchResultDto>(items, count, paging);
    }

    public async Task<PermitCreateResult> CreateAsync(PermitCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var workEntry = await CreatePermitFromDtoAsync(resource, currentUser, token).ConfigureAwait(false);

        await permitRepository.InsertAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        var result = new PermitCreateResult(workEntry.Id);

        // Send notification
        var template = Template.NewEntry;
        var notificationResult = await NotifyOwnerAsync(workEntry, template, token).ConfigureAwait(false);
        if (!notificationResult.Success) result.AddWarning(notificationResult.FailureMessage);

        return result;
    }

    public async Task UpdateAsync(Guid id, PermitUpdateDto resource, CancellationToken token = default)
    {
        var permit = await permitRepository.GetAsync(id, token).ConfigureAwait(false);
        permit.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await MapPermitDetailsAsync(permit, resource, token).ConfigureAwait(false);
        await permitRepository.UpdateAsync(permit, token: token).ConfigureAwait(false);
    }

    public async Task CloseAsync(PermitChangeStatusDto resource, CancellationToken token = default)
    {
        var workEntry = await permitRepository.GetAsync(resource.PermitId, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        permitManager.Close(workEntry, resource.Comments, currentUser);
        await permitRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);
    }

    public async Task<NotificationResult> ReopenAsync(PermitChangeStatusDto resource,
        CancellationToken token = default)
    {
        var workEntry = await permitRepository.GetAsync(resource.PermitId, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        permitManager.Reopen(workEntry, currentUser);
        await permitRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        // Send notification
        return await NotifyOwnerAsync(workEntry, Template.Reopened, token).ConfigureAwait(false);
    }

    public async Task DeleteAsync(PermitChangeStatusDto resource, CancellationToken token = default)
    {
        var workEntry = await permitRepository.GetAsync(resource.PermitId, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        permitManager.Delete(workEntry, resource.Comments, currentUser);
        await permitRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);
    }

    public async Task RestoreAsync(PermitChangeStatusDto resource, CancellationToken token = default)
    {
        var workEntry = await permitRepository.GetAsync(resource.PermitId, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        permitManager.Restore(workEntry, currentUser);
        await permitRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);
    }

    private async Task<Permit> CreatePermitFromDtoAsync(PermitCreateDto resource, ApplicationUser? currentUser,
        CancellationToken token)
    {
        var permit = permitManager.Create(currentUser);
        await MapPermitDetailsAsync(permit, resource, token).ConfigureAwait(false);
        return permit;
    }

    private async Task MapPermitDetailsAsync(Permit permit, IPermitCommandDto resource,
        CancellationToken token)
    {
        permit.ActionType = await actionTypeRepository.GetAsync(resource.ActionTypeId, token).ConfigureAwait(false);
        permit.Notes = resource.Notes;
    }

    private async Task<NotificationResult> NotifyOwnerAsync(Permit permit, Template template,
        CancellationToken token)
    {
        var recipient = permit.ReceivedBy;

        if (recipient is null)
            return NotificationResult.FailureResult("This Permit does not have an available recipient.");
        if (!recipient.Active)
            return NotificationResult.FailureResult("The Permit recipient is not an active user.");
        if (recipient.Email is null)
            return NotificationResult.FailureResult("The Permit recipient cannot be emailed.");

        return await notificationService.SendNotificationAsync(template, recipient.Email, permit, token)
            .ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        actionTypeRepository.Dispose();
        permitRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await permitRepository.DisposeAsync().ConfigureAwait(false);
        await actionTypeRepository.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
