using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Data
{
    public class Database
    {
        public async Task<MySqlConnection> GetConnectionAsync()
        {
            var conn = new MySqlConnection(DatabaseConfig.ConnectionString);
            await conn.OpenAsync();
            return conn;
        }
    }
}