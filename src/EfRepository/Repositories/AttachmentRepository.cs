using SWGW.Domain.Entities.Attachments;
using SWGW.EfRepository.DbContext;

namespace SWGW.EfRepository.Repositories;

public sealed class AttachmentRepository(AppDbContext context)
    : BaseRepository<Attachment, Guid, AppDbContext>(context), IAttachmentRepository;
