using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using Respawn;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Dapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Tests.Commands
{
    [Collection("Database collection")]
    public class CreateLibraryHandler : IAsyncLifetime
    {
        DatabaseFixture fixture;

        public CreateLibraryHandler(DatabaseFixture fixture)
        {
            this.fixture = fixture;
            User = insertNewUser();
        }

        public Task<IEnumerable<UserDto>> User;
        
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
        
//        private static Checkpoint disposeCheckpoint = new Checkpoint
//        {
//            SchemasToInclude = new[]
//            {
//                "plum",
//                "public"
//            },
//            DbAdapter = DbAdapter.Postgres
//            
//        };

        public async Task<IEnumerable<UserDto>> insertNewUser()
        {
            const string insertUserSql = @"
                INSERT INTO plum.users (display_name, google_claim_nameidentifier)
                VALUES ('testUser', 1)";
            
            const string selectUserIdSql = @"
                SELECT id FROM plum.users WHERE display_name = 'testUser'";

            using (var conn = fixture.ConnFactory.GetSqlConnection())
            {
                // Using respawn, reset to checkpoint
                await conn.OpenAsync();
                await checkpoint.Reset(conn);
                
                // Insert new user, then get the user id
                await conn.ExecuteAsync(insertUserSql);
                return await conn.QueryAsync<UserDto>(selectUserIdSql); // TODO - Replace UserDto with something in the ToBeRenamed.Tests namespace
            }
        }

        [Fact]
        public async void Should_Pass_When_LibraryIsValid()
        {
            
//            var connFactory = new TestSqlConnectionFactory(configuration);
            
            // Insert user
            
            
            const string insertLibrarySql = @"
                INSERT INTO plum.libraries (title, description, created_by)
                VALUES (@title, @description, @userId)";

            using (var cnn = fixture.ConnFactory.GetSqlConnection())
            {
                // Insert new user, then get the user id
//                await cnn.ExecuteAsync(insertUserSql);
//                var user = await cnn.QueryAsync<UserDto>(selectUserIdSql);
                
                // Create createLibrary requests and handlers to insert into database
//                var createLibrary = new CreateLibrary(1, "xUnitTitle", "xUnitDesc");
//                var createLibraryHandler = new CreateLibraryHandler(connFactory);
//                await createLibraryHandler.Handle(createLibrary, new System.Threading.CancellationToken());
                
                // Manually insert a library into the database
                var title = "xUnitTitle";
                var description = "xUnitDesc";
//                var userId = fixture.User.Result.First().Id;
                var userId = User.Result.First().Id;
                await cnn.ExecuteAsync(insertLibrarySql, new { userId, title, description });
               
            }
            
            
            
//            fixture.DisposeAll();

            // Reset DB to checkpoint
//            using (var conn = fixture.ConnFactory.GetSqlConnection())
//            {
//                await conn.OpenAsync();
//                
//                await disposeCheckpoint.Reset(conn);
//            }
            
            // Insert library into database
//            var createLibrary = new CreateLibrary(1, "xUnitTitle", "xUnitDesc");
//            var createLibraryHandler = new CreateLibraryHandler(connFactory);
//
//            await createLibraryHandler.Handle(createLibrary, new System.Threading.CancellationToken());
            
            // Get library from database
//            var userId = 1;
//            var title = "xUnitTitle";
//            var description = "xUnitDesc";

//            const string sql = @"
//                INSERT INTO plum.libraries (title, description, created_by)
//                VALUES (@title, @description, @userId)";

//            using (var cnn = connFactory.GetSqlConnection())
//            {
//                await cnn.ExecuteAsync(sql, new { userId, title, description });
//            }
        }
        [Fact]
        public async void Test2()
        {
            const string insertLibrarySql = @"
                INSERT INTO plum.libraries (title, description, created_by)
                VALUES (@title, @description, @userId)";

            using (var cnn = fixture.ConnFactory.GetSqlConnection())
            {
                // Using respawn, reset to checkpoint
                await cnn.OpenAsync();
                await checkpoint.Reset(cnn);
                
                // Insert new user, then get the user id
//                await cnn.ExecuteAsync(insertUserSql);
//                var user = await cnn.QueryAsync<UserDto>(selectUserIdSql);
                
                // Create createLibrary requests and handlers to insert into database
//                var createLibrary = new CreateLibrary(1, "xUnitTitle", "xUnitDesc");
//                var createLibraryHandler = new CreateLibraryHandler(connFactory);
//                await createLibraryHandler.Handle(createLibrary, new System.Threading.CancellationToken());
                
                // Manually insert a library into the database
                var title = "xUnitTitle";
                var description = "xUnitDesc";
                var userId = User.Result.First().Id;
                await cnn.ExecuteAsync(insertLibrarySql, new { userId, title, description });
               
            }
        }
        
        public async Task InitializeAsync()
        {
            using (var conn = fixture.ConnFactory.GetSqlConnection())
            {
                // Using respawn, reset to checkpoint
                await conn.OpenAsync();
                await checkpoint.Reset(conn);
            }
        }
        
        public async Task DisposeAsync()
        {
            using (var conn = fixture.ConnFactory.GetSqlConnection())
            {
                // Using respawn, reset to checkpoint
                await conn.OpenAsync();
                await checkpoint.Reset(conn);
            }
        }
    }
}
