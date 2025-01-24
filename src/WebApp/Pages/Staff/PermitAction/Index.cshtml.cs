using SWGW.AppServices.Permissions;

namespace SWGW.WebApp.Pages.Staff.PermitAction;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class PermitActionIndexModel : PageModel
{
    public RedirectToPageResult OnGet() => RedirectToPage("../Index");
}
