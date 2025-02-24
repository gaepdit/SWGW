﻿namespace SWGW.EfRepository.Migrations.DataMigration;

public static partial class Migrate
{
    // language=sql
    public const string AspNetRoles =
        """
        insert into dbo.AspNetRoles
            (Id,
             Name,
             NormalizedName,
             ConcurrencyStamp)
        select lower(Id) as Id,
               Name,
               NormalizedName,
               ConcurrencyStamp
        from dbo._archive_AspNetRoles;

        insert into dbo.AspNetRoles
            (Id,
             Name,
             NormalizedName,
             ConcurrencyStamp)
        values
            (newid(),
             'SiteMaintenance',
             upper('SiteMaintenance'),
             newid()),
            (newid(),
             'Staff',
             upper('Staff'),
             newid());
        """;
}
