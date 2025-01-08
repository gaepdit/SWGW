using SWGW.AppServices.EntryActions;
using SWGW.AppServices.WorkEntries;
using SWGW.WebApp.Pages.Staff.WorkEntries;

namespace WebAppTests;

internal static class PageModelHelpers
{
    public static DetailsModel BuildDetailsPageModel(
        IWorkEntryService? workEntryService = null,
        IEntryActionService? entryActionService = null,
        IAuthorizationService? authorizationService = null) =>
        new(workEntryService ?? Substitute.For<IWorkEntryService>(),
            entryActionService ?? Substitute.For<IEntryActionService>(),
            authorizationService ?? Substitute.For<IAuthorizationService>());
}
