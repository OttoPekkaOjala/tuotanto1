using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class PostiService
    {
        public async Task<List<Posti>> HaePostitAsync()
        {
            var lista = new List<Posti>();

            using var conn = Database.GetConnection(); // static call
            var cmd = new MySqlCommand("SELECT * FROM posti", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Posti
                {
                    Postinumero = reader.GetString(reader.GetOrdinal("postinumero")),
                    Toimipaikka = reader.GetString(reader.GetOrdinal("toimipaikka"))
                });
            }

            return lista;
        }
    }
}
