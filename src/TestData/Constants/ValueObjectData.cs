﻿using SWGW.Domain.ValueObjects;

namespace SWGW.TestData.Constants;

public static class ValueObjectData
{
    public static IncompleteAddress CompleteAddress => new()
    {
        Street = "123 Main St.",
        Street2 = "Box 456",
        City = "Town-ville",
        PostalCode = "98765-1234",
        State = "Georgia",
    };

    public static IncompleteAddress AlternateCompleteAddress => new()
    {
        Street = "2000 Alternate St.",
        Street2 = "Box 2000",
        City = "New-ville",
        PostalCode = "98765-2222",
        State = "Florida",
    };

    public static IncompleteAddress LessCompleteAddress => new()
    {
        Street = "456 Second St.",
        Street2 = null,
        City = "Alt-ville",
        PostalCode = "98765",
        State = "GA",
    };

    public static IncompleteAddress IncompleteAddress => new()
    {
        Street = "789 Third St.",
        Street2 = null,
        City = null,
        PostalCode = null,
        State = "GA",
    };
}
