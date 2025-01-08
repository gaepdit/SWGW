using System.Data;

namespace SWGW.EfRepository.DbConnection;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}
