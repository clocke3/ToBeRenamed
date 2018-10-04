using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace ToBeRenamed.Factories
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection GetSqlConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}