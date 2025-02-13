using SWGW.AppServices.Attachments;
using SWGW.AppServices.Attachments.Dto;
using SWGW.AppServices.Permits.Permissions;
using SWGW.AppServices.Permits.QueryDto;
using SWGW.AppServices.Permissions;
using SWGW.AppServices.Permissions.Helpers;
using SWGW.WebApp.Models;
using SWGW.WebApp.Platform.PageModelHelpers;
using SWGW.WebApp.Platform.Settings;

namespace SWGW.WebApp.Pages.Staff.Permits.Attachment;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class AttachmentDeleteModel(IAttachmentService attachmentService, IAuthorizationService authorization)
    : PageModel
{
    [BindProperty]
    public Guid AttachmentId { get; set; }

    public AttachmentViewDto AttachmentView { get; private set; } = null!;
    public int PermitId { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid? attachmentId)
    {
        if (attachmentId is null) return RedirectToPage("Index");

        var attachmentView = await attachmentService.FindAttachmentAsync(attachmentId.Value);
        if (attachmentView is null) return NotFound();

        var permitView = await attachmentService.FindPermitForAttachmentAsync(attachmentId.Value);
        if (permitView is null) return NotFound();

        if (!await UserCanDeleteAttachmentAsync(permitView)) return Forbid();

        AttachmentView = attachmentView;
        AttachmentId = attachmentId.Value;
        PermitId = permitView.Id;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var originalAttachment = await attachmentService.FindAttachmentAsync(AttachmentId, token);
        if (originalAttachment is null) return BadRequest();

        var permitView = await attachmentService.FindPermitForAttachmentAsync(AttachmentId, token);
        if (permitView is null || !await UserCanDeleteAttachmentAsync(permitView)) return BadRequest();

        await attachmentService.DeleteAttachmentAsync(originalAttachment, AppSettings.AttachmentServiceConfig, token);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Attachment successfully deleted.");
        return RedirectToPage("../Details", pageHandler: null, routeValues: new { permitView.Id },
            fragment: "attachments");
    }

    private Task<bool> UserCanDeleteAttachmentAsync(PermitViewDto item) =>
        authorization.Succeeded(User, item, PermitOperation.EditAttachments);
}
