﻿using SWGW.Domain.Entities.Permits;

namespace SWGW.AppServices.Permits.QueryDto;

public record PermitSearchResultDto
{
    public Guid Id { get; init; }
    public DateTimeOffset ReceivedDate { get; init; }
    public PermitStatus Status { get; init; }
    public bool Closed { get; init; }
    public DateTimeOffset? ClosedDate { get; init; }
    public string? EntryTypeName { get; init; }
    public bool IsDeleted { get; init; }
}
