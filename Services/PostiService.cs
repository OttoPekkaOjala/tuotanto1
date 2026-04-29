using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class PostiService
    {
        private readonly Database _db = new();

        public async Task<List<Posti>> HaePostitAsync()
        {
            var lista = new List<Posti>();
            using var conn = await _db.GetConnectionAsync();
            var cmd = new MySqlCommand("SELECT * FROM posti", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Posti
                {
                    Postinumero = reader.GetString("postinumero"),
                    Toimipaikka = reader.GetString("toimipaikka")
                });
            }
            return lista;
        }
    }
}