using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// tänne pitäs tulla se sql kai//

namespace tuotanto1.Data
{
    public static class Database
    {
        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(DatabaseConfig.ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
