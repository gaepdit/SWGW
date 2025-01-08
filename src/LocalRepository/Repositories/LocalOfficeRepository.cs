using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Identity;
using SWGW.LocalRepository.Identity;
using SWGW.TestData;

namespace SWGW.LocalRepository.Repositories;

public sealed class LocalOfficeRepository()
    : NamedEntityRepository<Office>(OfficeData.GetData), IOfficeRepository
{
    public LocalUserStore Staff { get; } = new();

    public Task<List<ApplicationUser>> GetStaffMembersListAsync(Guid id, bool includeInactive,
        CancellationToken token = default) =>
        Task.FromResult(Staff.Users
            .Where(user => user.Office != null && user.Office.Id == id)
            .Where(user => includeInactive || user.Active)
            .OrderBy(user => user.FamilyName).ThenBy(user => user.GivenName).ThenBy(user => user.Id)
            .ToList());
}
