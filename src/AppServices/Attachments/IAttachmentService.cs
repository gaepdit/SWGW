using SWGW.AppServices.Attachments.Dto;
using SWGW.AppServices.Permits.QueryDto;
using Microsoft.AspNetCore.Http;

namespace SWGW.AppServices.Attachments;

public interface IAttachmentService
{
    public const int MaxSimultaneousUploads = 10;

    // Attachment DTOs
    Task<AttachmentViewDto?> FindAttachmentAsync(Guid id, CancellationToken token = default);
    Task<AttachmentViewDto?> FindPublicAttachmentAsync(Guid id, CancellationToken token = default);
    Task<PermitViewDto?> FindPermitForAttachmentAsync(Guid attachmentId, CancellationToken token = default);

    // Attachment files
    Task<byte[]> GetAttachmentFileAsync(string fileId, bool getThumbnail, AttachmentServiceConfig config,
        CancellationToken token = default);

    Task DeleteAttachmentAsync(AttachmentViewDto attachmentView, AttachmentServiceConfig config,
        CancellationToken token = default);

    Task<int> SaveAttachmentsAsync(int permitId, List<IFormFile> files, AttachmentServiceConfig config,
        CancellationToken token = default);

    // Configuration
    public record AttachmentServiceConfig(string AttachmentsFolder, string ThumbnailsFolder, int ThumbnailSize);
}
