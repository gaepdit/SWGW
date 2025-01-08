using GaEpd.EmailService.EmailLogRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SWGW.Domain.Entities.EntryActions;
using SWGW.Domain.Entities.EntryTypes;
using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Entities.WorkEntries;
using SWGW.EfRepository.DbConnection;
using SWGW.EfRepository.DbContext;
using SWGW.EfRepository.Repositories;
using SWGW.LocalRepository.Repositories;
using SWGW.WebApp.Platform.Settings;

namespace SWGW.WebApp.Platform.AppConfiguration;

public static class DataPersistence
{
    public static IServiceCollection AddDataPersistence(this IServiceCollection services,
        ConfigurationManager configuration,
        IWebHostEnvironment environment)
    {
        // When configured, use in-memory data; otherwise use a SQL Server database.
        if (AppSettings.DevSettings.UseInMemoryData)
        {
            // Use in-memory data for all repositories.
            services
                .AddSingleton<IEmailLogRepository, LocalEmailLogRepository>()
                .AddSingleton<IEntryActionRepository, LocalEntryActionRepository>()
                .AddSingleton<IEntryTypeRepository, LocalEntryTypeRepository>()
                .AddSingleton<IOfficeRepository, LocalOfficeRepository>()
                .AddSingleton<IWorkEntryRepository, LocalWorkEntryRepository>();

            return services;
        }

        // When in-memory data is disabled, use a database connection.
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            // In-memory database (not recommended)
            services.AddDbContext<AppDbContext>(builder => builder.UseInMemoryDatabase("TEMP_DB"));
        }
        else
        {
            // Entity Framework context
            services.AddDbContext<AppDbContext>(dbBuilder =>
            {
                dbBuilder
                    .UseSqlServer(connectionString, sqlServerOpts => sqlServerOpts.EnableRetryOnFailure())
                    .ConfigureWarnings(builder => builder.Throw(RelationalEventId.MultipleCollectionIncludeWarning));

                if (environment.IsDevelopment()) dbBuilder.EnableSensitiveDataLogging();
            });

            // Dapper DB connection
            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>(_ =>
                new DbConnectionFactory(connectionString));
        }

        // Repositories
        services
            .AddScoped<IEmailLogRepository, EmailLogRepository>()
            .AddScoped<IEntryActionRepository, EntryActionRepository>()
            .AddScoped<IEntryTypeRepository, EntryTypeRepository>()
            .AddScoped<IOfficeRepository, OfficeRepository>()
            .AddScoped<IWorkEntryRepository, WorkEntryRepository>();

        return services;
    }
}
