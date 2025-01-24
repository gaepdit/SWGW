using SWGW.AppServices.PermitActions.Dto;

namespace SWGW.AppServices.PermitActions;

public interface IActionService : IDisposable, IAsyncDisposable
{
    Task<Guid> CreateAsync(ActionCreateDto resource, CancellationToken token = default);
    Task<ActionViewDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<ActionUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(Guid id, ActionUpdateDto resource, CancellationToken token = default);
    Task DeleteAsync(Guid entryActionId, CancellationToken token = default);
    Task RestoreAsync(Guid entryActionId, CancellationToken token = default);
}
