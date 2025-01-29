using SWGW.Domain.Entities.PermitActions;
using System.Linq.Expressions;

namespace SWGW.Domain.Entities.Permits;

public interface IPermitRepository : IRepository<Permit>
{
    // Permit ID

    // Will return the next available Permit ID if the repository requires it for adding new entities (local repository).
    // Will return null if the repository creates a new ID on insert (Entity Framework).
    int? GetNextId();


    /// <summary>
    /// Returns the <see cref="Permit"/> with the given <paramref name="id"/> and includes all additional
    /// properties (<see cref="PermitAction"/>). Returns null if there are no matches.
    /// </summary>
    /// <param name="id">The Id of the Permit.</param>
    /// <param name="includeDeletedActions">Whether to include deleted Permit Actions in the result.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="InvalidOperationException">Thrown if there are multiple matches.</exception>
    /// <returns>A Permit entity.</returns>
    Task<Permit?> FindIncludeAllAsync(Guid id, bool includeDeletedActions = false, CancellationToken token = default);


    // Specialized Permit queries

    /// <summary>
    /// Returns the <see cref="Permit"/> with the given <paramref name="id"/> and includes all additional
    /// properties (<see cref="PermitAction"/>, <see cref="Attachment"/>, & <see cref="PermittTransition"/>).
    /// Returns null if there are no matches.
    /// </summary>
    /// <param name="id">The Id of the Permit.</param>
    /// <param name="includeDeletedActions">Whether to include deleted Permit Actions in the result.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="InvalidOperationException">Thrown if there are multiple matches.</exception>
    /// <returns>A Permit entity.</returns>
    Task<Permit?> FindIncludeAllAsync(int id, bool includeDeletedActions = false, CancellationToken token = default);

    /// <summary>
    /// Returns the <see cref="Permit"/> matching the conditions of <paramref name="predicate"/>. If Permit is
    /// closed, then the Permit also includes additional properties (<see cref="PermitAction"/>,
    /// <see cref="Attachment"/>, & <see cref="PermitTransition"/>). Returns null if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="InvalidOperationException">Thrown if there are multiple matches.</exception>
    /// <returns>A Permit entity.</returns>
   

    /// <summary>
    /// Returns a read-only collection of <see cref="Permit"/> matching the conditions of the <paramref name="predicate"/>.
    /// Each Permit in the collection includes the most recent <see cref="PermitAction"/> for that Permit.
    /// Returns an empty collection if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="sorting"></param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of Permit with the most recent Action for each.</returns>
    Task<IReadOnlyCollection<Permit>> GetListWithMostRecentActionAsync(Expression<Func<Permit, bool>> predicate,
        string sorting = "", CancellationToken token = default);

   
}
