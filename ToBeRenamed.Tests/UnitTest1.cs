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
            },
            DbAdapter = DbAdapter.Postgres
        };

        

        [Fact]
        public async void Test1()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var connFactory = new TestSqlConnectionFactory(configuration);

            using (var conn = connFactory.GetSqlConnection())
            {
                await conn.OpenAsync();
                
                await checkpoint.Reset(conn);
            }
            
//            var createLibrary = new CreateLibrary(1, "xUnitTitle", "xUnitDesc");
//            var createLibraryHandler = new CreateLibraryHandler(connFactory);
//
//            await createLibraryHandler.Handle(createLibrary, new System.Threading.CancellationToken());

            const string sql2 = @"
                INSERT INTO plum.users (display_name, google_claim_nameidentifier)
                VALUES (1, 1)";

            using (var cnn = connFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql2);
            }
            
            var userId = 16;
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
        
        [Fact]
        public async void Test2()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connFactory = new TestSqlConnectionFactory(configuration);

            using (var conn = connFactory.GetSqlConnection())
            {
                await conn.OpenAsync();
                
                await checkpoint.Reset(conn);
            }
            
            const string sql2 = @"
                INSERT INTO plum.users (display_name, google_claim_nameidentifier)
                VALUES (1, 1)";

            using (var cnn = connFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql2);
            }
            
            var userId = 17;
            var title = "xUnitTitle2";
            var description = "xUnitDesc2";

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
