using SWGW.Domain.Entities.Attachments;
using SWGW.TestData;

namespace SWGW.LocalRepository.Repositories;

public sealed class LocalAttachmentRepository() 
    : BaseRepository<Attachment, Guid>(AttachmentData.GetAttachments), IAttachmentRepository;
