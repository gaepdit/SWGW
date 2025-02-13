using SWGW.Domain.Entities.Permits;
using SWGW.Domain.Identity;
using Microsoft.AspNetCore.Http;

namespace SWGW.Domain.Entities.Attachments;

public class AttachmentManager : IAttachmentManager
{
    public Attachment Create(IFormFile formFile, Permit permit, ApplicationUser? user) =>
        new(Guid.NewGuid())
        {
            Permit = permit,
            FileName = Path.GetFileName(formFile.FileName).Trim(),
            FileExtension = Path.GetExtension(formFile.FileName),
            Size = formFile.Length,
            UploadedBy = user,
        };
}
