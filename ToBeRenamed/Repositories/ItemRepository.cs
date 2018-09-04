using Dapper;
using System.Collections.Generic;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Repositories
{
    public class ItemRepository
    {
        private readonly SqlConnectionFactory _sqlConnectionFactory;

        public ItemRepository(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public IEnumerable<ItemDto> GetAllItems()
        {
            const string sql = "SELECT id, name FROM items";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return cnn.Query<ItemDto>(sql);
            }
        }
    }
}
