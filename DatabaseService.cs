using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tuotanto1
{
    public class DatabaseService
    {
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var conn = new MySqlConnection(DatabaseConfig.ConnectionString);
                await conn.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
