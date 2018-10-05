using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Data.Common;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Tests
{
    // TODO change back to implement IDbConnection
    public class TestSqlConnectionFactory
    {
        private readonly string _connectionString;

        public TestSqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:TestConnection"];
        }

        public DbConnection GetSqlConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}