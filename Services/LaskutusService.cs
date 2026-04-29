using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class LaskutusService
    {
        public async Task<List<Lasku>> HaeKaikkiLaskutAsync()
        {
            var lista = new List<Lasku>();

            using var conn = Database.GetConnection(); // static call
            var cmd = new MySqlCommand("SELECT * FROM lasku", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Lasku
                {
                    LaskuId = reader.GetInt32(reader.GetOrdinal("lasku_id")),
                    VarausId = reader.GetInt32(reader.GetOrdinal("varaus_id")),
                    Summa = reader.GetDecimal(reader.GetOrdinal("summa")),
                    Alv = reader.GetDecimal(reader.GetOrdinal("alv")),
                    Maksettu = reader.GetBoolean(reader.GetOrdinal("maksettu"))
                });
            }

            return lista;
        }

        public async Task<bool> LisaaLaskuAsync(Lasku lasku)
        {
            using var conn = Database.GetConnection(); // static call

            var cmd = new MySqlCommand(
                "INSERT INTO lasku (varaus_id, summa, alv, maksettu) VALUES (@varaus_id, @summa, @alv, @maksettu)",
                conn);

            cmd.Parameters.AddWithValue("@varaus_id", lasku.VarausId);
            cmd.Parameters.AddWithValue("@summa", lasku.Summa);
            cmd.Parameters.AddWithValue("@alv", lasku.Alv);
            cmd.Parameters.AddWithValue("@maksettu", lasku.Maksettu);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}


