using SWGW.Domain.Entities.Attachments;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace SWGW.AppServices.Attachments;

internal static class AttachmentFilters
{
    public static Expression<Func<Attachment, bool>> IdPredicate(Guid id) =>
        PredicateBuilder.True<Attachment>().WithId(id).ExcludeDeleted();

    public static Expression<Func<Attachment, bool>> PublicIdPredicate(Guid id) =>
        IdPredicate(id).permitIsClosed();

    private static Expression<Func<Attachment, bool>> permitIsClosed(
        this Expression<Func<Attachment, bool>> predicate) =>
        predicate.And(attachment => attachment.Permit.PermitClosed);
}
