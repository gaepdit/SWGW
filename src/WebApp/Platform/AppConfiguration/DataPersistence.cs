using GaEpd.EmailService.EmailLogRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SWGW.Domain.Entities.PermitActions;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Entities.Permits;
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
                .AddSingleton<IActionRepository, LocalPermitActionRepository>()
                .AddSingleton<IActionTypeRepository, LocalActionTypeRepository>()
                .AddSingleton<IOfficeRepository, LocalOfficeRepository>()
                .AddSingleton<IPermitRepository, LocalPermitRepository>();

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
            .AddScoped<IActionRepository, PermitActionRepository>()
            .AddScoped<IActionTypeRepository, ActionTypeRepository>()
            .AddScoped<IOfficeRepository, OfficeRepository>()
            .AddScoped<IPermitRepository, PermitRepository>();

        return services;
    }
}
