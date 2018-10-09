using Dapper;
using Respawn;
using Xunit;

namespace ToBeRenamed.Tests
{
    [Collection("Database collection")]
    public class UnitTest2
    {
        DatabaseFixture fixture;

        public UnitTest2(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
        
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
        public void Test2()
        {
            fixture.resetDatabase(checkpoint);
            
            var userId = fixture.User.Id;
            string title = "xUnitTitle";
            var description = "xUnitDesc";
            
            const string insertLibrarySql = @"
                INSERT INTO plum.libraries (title, description, created_by)
                VALUES (@title, @description, @userId)";
            
            using (var cnn = fixture.ConnFactory.GetSqlConnection())
            {
                cnn.Execute(insertLibrarySql, new { userId, title, description });
            }
        }
    }
}
