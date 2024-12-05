using Microsoft.Extensions.DependencyInjection;
using MyApp.AppServices.AutoMapper;

namespace MyApp.AppServices.RegisterServices;

public static class AutoMapperProfiles
{
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        // Add AutoMapper profiles
       return services.AddAutoMapper(expression => expression.AddProfile<AutoMapperProfile>());
    }
}
