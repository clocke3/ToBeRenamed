using System.Data;

namespace ToBeRenamed.Factories
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetSqlConnection();
    }
}