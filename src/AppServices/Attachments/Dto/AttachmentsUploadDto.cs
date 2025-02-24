using SWGW.AppServices.Attachments.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SWGW.AppServices.Attachments.Dto;

public record AttachmentsUploadDto
{
    [Required(ErrorMessage = "No files were selected.")]
    [ValidateFileTypes]
    [FilesNotEmpty]
    [FilesRequired]
    [MaxNumberOfFiles(IAttachmentService.MaxSimultaneousUploads)]
    public List<IFormFile> Files { get; } = [];
}
