﻿using SWGW.Domain.Entities.Permits;

namespace SWGW.AppServices.Permits;

public class PermitCreateResult
{
    /// <summary>
    /// Returns <see cref="PermitCreateResult"/> indicating a successfully created <see cref="Permit"/>.
    /// </summary>
    /// <param name="permitId">The ID of the new Permit.</param>
    /// <returns><see cref="PermitCreateResult"/> indicating a successful operation.</returns>
    public PermitCreateResult(int permitId) => PermitId = permitId;

    /// <summary>
    /// If the <see cref="Permit"/> is successfully created, contains the ID of the new Permit. 
    /// </summary>
    /// <value>The Permit ID if the operation succeeded, otherwise null.</value>
    public int? PermitId { get; protected init; }

    /// <summary>
    /// <see cref="List{T}"/> of <see cref="string"/> containing warnings that occurred during the operation.
    /// </summary>
    /// <value>A <see cref="List{T}"/> of <see cref="string"/> instances.</value>
    public List<string> Warnings { get; } = [];

    /// <summary>
    /// Flag indicating whether warnings were generated.
    /// </summary>
    /// <value>True if warnings were generated, otherwise false.</value>
    public bool HasWarnings { get; private set; }

    /// <summary>
    /// Adds a warning to result.
    /// </summary>
    /// <param name="warning">The warning generated.</param>
    public void AddWarning(string warning)
    {
        HasWarnings = true;
        Warnings.Add(warning);
    }
}
