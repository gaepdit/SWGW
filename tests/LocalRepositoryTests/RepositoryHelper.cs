using SWGW.LocalRepository.Identity;
using SWGW.LocalRepository.Repositories;
using SWGW.TestData;
using SWGW.TestData.Identity;

namespace LocalRepositoryTests;

public static class RepositoryHelper
{
    public static LocalUserStore GetUserStore()
    {
        ClearAllStaticData();
        return new LocalUserStore();
    }

    public static LocalOfficeRepository GetOfficeRepository()
    {
        ClearAllStaticData();
        return new LocalOfficeRepository();
    }

    private static void ClearAllStaticData()
    {
        OfficeData.ClearData();
        UserData.ClearData();
        PermitActionData.ClearData();
        PermitData.ClearData();
    }
}
