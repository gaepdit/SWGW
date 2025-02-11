using GaEpd.AppLibrary.Domain.Predicates;
using SWGW.AppServices.Permits.QueryDto;
using SWGW.Domain.Entities.Permits;
using System.Linq.Expressions;

namespace SWGW.AppServices.Permits;

internal static class PermitFilters
{
    public static Expression<Func<Permit, bool>> SearchPredicate(PermitSearchDto spec) =>
        PredicateBuilder.True<Permit>()
            //.ByStatus(spec.Status)
            .ByDeletedStatus(spec.DeletedStatus)
            .FromReceivedDate(spec.ReceivedFrom)
            .ToReceivedDate(spec.ReceivedTo)
            .ReceivedBy(spec.ReceivedBy)
            .IsActionType(spec.ActionType)
            .ContainsText(spec.Text);

    private static Expression<Func<Permit, bool>> IsClosed(this Expression<Func<Permit, bool>> predicate) =>
        predicate.And(entry => entry.Closed);

    private static Expression<Func<Permit, bool>> IsOpen(this Expression<Func<Permit, bool>> predicate) =>
        predicate.And(permit => !permit.Closed);

    private static Expression<Func<Permit, bool>> ByStatus(this Expression<Func<Permit, bool>> predicate,
        PermitStatus? input) => input switch
    {
        PermitStatus.Active => predicate.IsOpen(),
        PermitStatus.Void => predicate.IsClosed(),
        _ => predicate,
    };

    private static Expression<Func<Permit, bool>> ByDeletedStatus(this Expression<Func<Permit, bool>> predicate,
        SearchDeleteStatus? input) => input switch
    {
        SearchDeleteStatus.All => predicate,
        SearchDeleteStatus.Deleted => predicate.And(entry => entry.IsDeleted),
        _ => predicate.And(permit => !permit.IsDeleted),
    };

    private static Expression<Func<Permit, bool>> FromReceivedDate(this Expression<Func<Permit, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ReceivedDate.Date >= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<Permit, bool>> ToReceivedDate(this Expression<Func<Permit, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ReceivedDate.Date <= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<Permit, bool>> ReceivedBy(this Expression<Func<Permit, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entry => entry.ReceivedBy != null && entry.ReceivedBy.Id == input);

    private static Expression<Func<Permit, bool>> IsActionType(this Expression<Func<Permit, bool>> predicate,
        Guid? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ActionType != null && entry.ActionType.Id == input);

    private static Expression<Func<Permit, bool>> ContainsText(this Expression<Func<Permit, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(entry => entry.Notes.Contains(input));
}
