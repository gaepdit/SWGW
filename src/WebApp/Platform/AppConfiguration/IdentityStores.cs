using Microsoft.AspNetCore.Identity;
using SWGW.AppServices.Staff;
using SWGW.AppServices.UserServices;
using SWGW.Domain.Identity;
using SWGW.EfRepository.DbContext;
using SWGW.LocalRepository.Identity;
using SWGW.WebApp.Platform.Settings;

namespace SWGW.WebApp.Platform.AppConfiguration;

public static class IdentityStores
{
    public static void AddIdentityStores(this IServiceCollection services)
    {
        var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole>();

        // When running locally, you have the option to use in-memory data or a database.
        if (AppSettings.DevSettings.UseInMemoryData)
        {
            // Add local UserStore and RoleStore.
            services.AddSingleton<IUserStore<ApplicationUser>, LocalUserStore>();
            services.AddSingleton<IRoleStore<IdentityRole>, LocalRoleStore>();
        }
        else
        {
            // Add EF identity stores.
            identityBuilder.AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
        }

        // Add staff and user services.
        services.AddTransient<IStaffService, StaffService>();
        services.AddScoped<IUserService, UserService>();
    }
}
