using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Respawn;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;
using Xunit;

namespace ToBeRenamed.Tests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public DatabaseFixture()
        {
//            User = insertNewUser();
        }
        
        // Contains test database connection string
        private static IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        public ISqlConnectionFactory ConnFactory = new TestSqlConnectionFactory(configuration);

        public Task<IEnumerable<UserDto>> User;

        public async Task<IEnumerable<UserDto>> insertNewUser()
        {
            const string insertUserSql = @"
                INSERT INTO plum.users (display_name, google_claim_nameidentifier)
                VALUES ('testUser', 1)";
            
            const string selectUserIdSql = @"
                SELECT id FROM plum.users WHERE display_name = 'testUser'";

            using (var conn = ConnFactory.GetSqlConnection())
            {
                // Using respawn, reset to checkpoint
                await conn.OpenAsync();
                await checkpoint.Reset(conn);
                
                // Insert new user, then get the user id
                await conn.ExecuteAsync(insertUserSql);
                return await conn.QueryAsync<UserDto>(selectUserIdSql); // TODO - Replace UserDto with something in the ToBeRenamed.Tests namespace
            }
        }
        
        private static Checkpoint checkpoint = new Checkpoint
        {
            SchemasToInclude = new[]
            {
                "plum",
                "public"
            },
            DbAdapter = DbAdapter.Postgres
        };

        public async Task InitializeAsync()
        {
            using (var conn = ConnFactory.GetSqlConnection())
            {
                // Using respawn, reset to checkpoint
                await conn.OpenAsync();
                await checkpoint.Reset(conn);
            }
        }
        
        public async Task DisposeAsync()
        {
            using (var conn = ConnFactory.GetSqlConnection())
            {
                // Using respawn, reset to checkpoint
                await conn.OpenAsync();
                await checkpoint.Reset(conn);
            }
        }
    }
}