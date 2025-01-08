using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;

namespace SWGW.WebApp.Pages;

[AllowAnonymous]
public class SupportModel(IAuthorizationService authorization) : PageModel
{
    public bool ActiveUser { get; private set; }
    public string? Version { get; private set; }

    public async Task OnGetAsync()
    {
        ActiveUser = await authorization.Succeeded(User, Policies.ActiveUser);
        Version = GetType().Assembly.GetName().Version?.ToString(fieldCount: 3);
    }
}
