using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class PalveluService
    {
        public async Task<List<Palvelu>> HaeKaikkiPalvelutAsync()
        {
            var lista = new List<Palvelu>();

            using var conn = Database.GetConnection(); // static call
            var cmd = new MySqlCommand("SELECT * FROM palvelu", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Palvelu
                {
                    PalveluId = reader.GetInt32(reader.GetOrdinal("palvelu_id")),
                    AlueId = reader.GetInt32(reader.GetOrdinal("alue_id")),
                    Nimi = reader.GetString(reader.GetOrdinal("nimi")),
                    Kuvaus = reader.GetString(reader.GetOrdinal("kuvaus")),
                    Hinta = reader.GetDecimal(reader.GetOrdinal("hinta")),
                    Alv = reader.GetDecimal(reader.GetOrdinal("alv"))
                });
            }

            return lista;
        }
    }
}

