using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class PalveluService
    {
        private readonly Database _db = new();

        public async Task<List<Palvelu>> HaeKaikkiPalvelutAsync()
        {
            var lista = new List<Palvelu>();
            using var conn = await _db.GetConnectionAsync();
            var cmd = new MySqlCommand("SELECT * FROM palvelu", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Palvelu
                {
                    PalveluId = reader.GetInt32("palvelu_id"),
                    AlueId = reader.GetInt32("alue_id"),
                    Nimi = reader.GetString("nimi"),
                    Kuvaus = reader.GetString("kuvaus"),
                    Hinta = reader.GetDecimal("hinta"),
                    Alv = reader.GetDecimal("alv")
                });
            }
            return lista;
        }
    }
}
