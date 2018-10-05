using System;
using Xunit;
using Respawn;
using System.IO;
using Dapper;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using ToBeRenamed.Commands;

namespace ToBeRenamed.Tests
{
    public class UnitTest1
    {
        private static Checkpoint checkpoint = new Checkpoint
        {
            SchemasToInclude = new[]
            {
                "plum",
                "public"
            },TablesToIgnore = new[]
            {
                "users"
            },
            DbAdapter = DbAdapter.Postgres
        };

        

        [Fact]
        public async void Test1()
        {
            // Get test connection string
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var connFactory = new TestSqlConnectionFactory(configuration);
            
            // Insert user
            const string insertUserSql = @"
                INSERT INTO plum.users (display_name, google_claim_nameidentifier)
                VALUES (1, 1)";

            using (var cnn = connFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(insertUserSql);
            }

            // Reset DB to checkpoint
            using (var conn = connFactory.GetSqlConnection())
            {
                await conn.OpenAsync();
                
                await checkpoint.Reset(conn);
            }
            
            // Insert library into database
            var createLibrary = new CreateLibrary(1, "xUnitTitle", "xUnitDesc");
            var createLibraryHandler = new CreateLibraryHandler(connFactory);

            await createLibraryHandler.Handle(createLibrary, new System.Threading.CancellationToken());
            
            // Get library from database
            var userId = 1;
            var title = "xUnitTitle";
            var description = "xUnitDesc";

            const string sql = @"
                INSERT INTO plum.libraries (title, description, created_by)
                VALUES (@title, @description, @userId)";

            using (var cnn = connFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new { userId, title, description });
            }
        }
    }
}
