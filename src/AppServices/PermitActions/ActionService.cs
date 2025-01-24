using AutoMapper;
using SWGW.AppServices.PermitActions.Dto;
using SWGW.AppServices.UserServices;
using SWGW.Domain.Entities.PermitActions;
using SWGW.Domain.Entities.Perimits;
using SWGW.AppServices.PermitActions;

namespace SWGW.AppServices.PermitActions;

public sealed class ActionService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IUserService userService,
    IPermitRepository permitRepository,
    IPermitManager permitManager,
     IActionRepository actionRepository,
    IActionRepository actionTypeRepository)
    : IActionService
{
    public async Task<Guid> CreateAsync(ActionCreateDto resource, CancellationToken token = default)
    {
        var workEntry = await permitRepository.GetAsync(resource.PermitId, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var entryAction = permitManager.CreateAction(workEntry, currentUser);

        entryAction.ActionDate = resource.ActionDate!.Value;
        entryAction.Comments = resource.Comments;

        await actionTypeRepository.InsertAsync(entryAction, token: token).ConfigureAwait(false);
        return entryAction.Id;
    }

    public async Task<ActionViewDto?> FindAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ActionViewDto>(
            await actionTypeRepository.FindAsync(id, token).ConfigureAwait(false));

    public async Task<ActionUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ActionUpdateDto>(
            await actionTypeRepository.FindAsync(action => action.Id == id && !action.IsDeleted, token)
                .ConfigureAwait(false));

    public async Task UpdateAsync(Guid id, ActionUpdateDto resource, CancellationToken token = default)
    {
        var entryAction = await actionTypeRepository.GetAsync(id, token).ConfigureAwait(false);
        entryAction.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        entryAction.ActionDate = resource.ActionDate!.Value;
        entryAction.Comments = resource.Comments;

        await actionTypeRepository.UpdateAsync(entryAction, token: token).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid entryActionId, CancellationToken token = default)
    {
        var entryAction = await actionTypeRepository.GetAsync(entryActionId, token).ConfigureAwait(false);
        entryAction.SetDeleted((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await actionTypeRepository.UpdateAsync(entryAction, token: token).ConfigureAwait(false);
    }

    public async Task RestoreAsync(Guid entryActionId, CancellationToken token = default)
    {
        var entryAction = await actionTypeRepository.GetAsync(entryActionId, token).ConfigureAwait(false);
        entryAction.SetNotDeleted();
        await actionTypeRepository.UpdateAsync(entryAction, token: token).ConfigureAwait(false);
    }

    public void Dispose()
    {
        permitRepository.Dispose();
        actionRepository.Dispose();
        actionTypeRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await permitRepository.DisposeAsync().ConfigureAwait(false);
        await actionRepository.DisposeAsync().ConfigureAwait(false);
        await actionTypeRepository.DisposeAsync().ConfigureAwait(false);
    }
}
