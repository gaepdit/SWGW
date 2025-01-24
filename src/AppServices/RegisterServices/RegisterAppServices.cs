using GaEpd.EmailService;
using Microsoft.Extensions.DependencyInjection;
using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.DataExport;
using SWGW.AppServices.Notifications;
using SWGW.AppServices.Offices;
using SWGW.AppServices.Perimits;
using SWGW.AppServices.PermitActions;
using SWGW.Domain.Entities.ActionTypes;
using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Entities.Perimits;

namespace SWGW.AppServices.RegisterServices;

public static class RegisterAppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        //Permits
        services.AddScoped<IPermitManager, PermitManager>();
        services.AddScoped<IPermitService, PermitService>();

        // Permit Actions
        services.AddScoped<IActionService, ActionService>();

        // Action Types
        services.AddScoped<IActionTypeManager, ActionTypeManager>();
        services.AddScoped<IActionTypeService, ActionTypeService>();
        
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
