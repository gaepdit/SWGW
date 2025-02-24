using SWGW.Domain.Entities.Offices;
using SWGW.Domain.Identity;
using SWGW.EfRepository.DbContext;

namespace SWGW.EfRepository.Repositories;

public sealed class OfficeRepository(AppDbContext context) :
    NamedEntityRepository<Office, AppDbContext>(context), IOfficeRepository
{
    public Task<List<ApplicationUser>> GetStaffMembersListAsync(Guid id, bool includeInactive,
        CancellationToken token = default) =>
        Context.Set<ApplicationUser>().AsNoTracking()
            .Where(user => user.Office != null && user.Office.Id == id)
            .Where(user => includeInactive || user.Active)
            .OrderBy(user => user.FamilyName).ThenBy(user => user.GivenName).ThenBy(user => user.Id)
            .ToListAsync(token);
}
