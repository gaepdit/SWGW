using SWGW.AppServices.Staff.Dto;
using SWGW.AppServices.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWGW.AppServices.Attachments.Dto;

public record AttachmentViewDto
{
    public Guid Id { get; init; }

    public string FileName { get; init; } = string.Empty;

    [JsonIgnore]
    public string FileExtension { get; init; } = string.Empty;

    [JsonIgnore]
    public string FileId => string.Concat(Id.ToString(), FileExtension);

    [Display(Name = "Size in bytes")]
    public long Size { get; init; }

    public string SizeDescription => FileSize.ToFileSizeString(Size);

    [Display(Name = "Uploaded By")]
    public StaffViewDto? UploadedBy { get; init; }

    public DateTimeOffset UploadedDate { get; init; }

    public bool IsImage { get; init; }
    public bool IsForPublic { get; set; }
}
