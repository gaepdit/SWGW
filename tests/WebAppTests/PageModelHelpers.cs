using SWGW.AppServices.ActionTypes;
using SWGW.AppServices.Perimits;
using SWGW.AppServices.PermitActions;
using SWGW.WebApp.Pages.Staff.Perimits;


namespace WebAppTests;

internal static class PageModelHelpers
{
    public static DetailsModel BuildDetailsPageModel(
        IPermitService? permitService = null,
        IActionService? permitActionService = null,
        IAuthorizationService? authorizationService = null) =>
        new(permitService ?? Substitute.For<IPermitService>(),
            permitActionService ?? Substitute.For<IActionService>(),
            authorizationService ?? Substitute.For<IAuthorizationService>());
}
