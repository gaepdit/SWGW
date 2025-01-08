using GaEpd.EmailService;
using Microsoft.Extensions.DependencyInjection;
using SWGW.AppServices.DataExport;
using SWGW.AppServices.EntryActions;
using SWGW.AppServices.EntryTypes;
using SWGW.AppServices.Notifications;
using SWGW.AppServices.Offices;
using SWGW.AppServices.WorkEntries;
using SWGW.Domain.Entities.EntryTypes;
using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Entities.WorkEntries;

namespace SWGW.AppServices.RegisterServices;

public static class RegisterAppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        // Work Entries
        services.AddScoped<IWorkEntryManager, WorkEntryManager>();
        services.AddScoped<IWorkEntryService, WorkEntryService>();

        // Entry Actions
        services.AddScoped<IEntryActionService, EntryActionService>();

        // Entry Types
        services.AddScoped<IEntryTypeManager, EntryTypeManager>();
        services.AddScoped<IEntryTypeService, EntryTypeService>();
        
        // Notifications
        services.AddScoped<INotificationService, NotificationService>();

        // Offices
        services.AddScoped<IOfficeManager, OfficeManager>();
        services.AddScoped<IOfficeService, OfficeService>();

        // Data Export
        services.AddScoped<ISearchResultsExportService, SearchResultsExportService>();

        return services;
    }
}
