using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.PermitActions;
using SWGW.Domain.Identity;

namespace SWGW.Domain.Entities.Perimits;

public interface IPermitManager
{
    /// <summary>
    /// Creates a new <see cref="Permit"/>.
    /// </summary>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The Permit that was created.</returns>
    Permit Create(ApplicationUser? user);

    /// <summary>
    /// Creates a new <see cref="EntryAction"/>.
    /// </summary>
    /// <param name="permit">The <see cref="Permit"/> this Action belongs to.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The Permit Action that was created.</returns>
   PermitAction CreateAction(Permit permit, ActionType actionType, ApplicationUser? user);

    PermitAction CreateAction(Permit permit,  ApplicationUser? user);


    /// <summary>
    /// Updates the properties of a <see cref="Permit"/> to indicate that it was reviewed and closed.
    /// </summary>
    /// <param name="permit">The Permit that was closed.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Close(Permit permit, string? comment, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a closed <see cref="Permit"/> to indicate that it was reopened.
    /// </summary>
    /// <param name="permit">The Permit that was reopened.</param>
    /// <param name="user">The user committing the change.</param>
    void Reopen(Permit permit, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="Permit"/> to indicate that it was deleted.
    /// </summary>
    /// <param name="permit">The Permit which was deleted.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Delete(Permit permit, string? comment, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a deleted <see cref="Permit"/> to indicate that it was restored.
    /// </summary>
    /// <param name="permit">The Permit which was restored.</param>
    /// <param name="user">The user committing the change.</param>
    void Restore(Permit permit, ApplicationUser? user);
}
