﻿using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;

namespace SWGW.WebApp.Pages.Account;

[AllowAnonymous]
public class LoginModel(IAuthorizationService authorization) : PageModel
{
    public string? ReturnUrl { get; private set; }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            if (!await authorization.Succeeded(User, Policies.ActiveUser)) return RedirectToPage("Logout");
            return string.IsNullOrEmpty(returnUrl) ? RedirectToPage("/Index") : LocalRedirect(returnUrl);
        }

        ReturnUrl = returnUrl;
        return Page();
    }
}
