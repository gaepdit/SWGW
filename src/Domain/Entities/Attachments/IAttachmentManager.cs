using SWGW.Domain.Entities.Permits;
using SWGW.Domain.Identity;
using Microsoft.AspNetCore.Http;

namespace SWGW.Domain.Entities.Attachments;

public interface IAttachmentManager
{
    /// <summary>
    /// Creates a new <see cref="Attachment"/>.
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="permit">The <see cref="CompPermitlaint"/> the file is attached to.</param>
    /// <param name="user">The user adding the Attachment.</param>
    /// <returns>The Attachment that was created.</returns>
    public Attachment Create(IFormFile formFile, Permit permit, ApplicationUser? user);
}
