using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class RaportointiService
    {
        private readonly Database _db = new();

        // Esimerkki: hae kaikki laskut ja laske kokonaissumma
        public async Task<decimal> HaeKokonaissummaAsync()
        {
            using var conn = await _db.GetConnectionAsync();
            var cmd = new MySqlCommand("SELECT SUM(summa) FROM lasku", conn);
            var result = await cmd.ExecuteScalarAsync();

            return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
        }

        // Esimerkki: hae laskut, jotka eivät ole maksettu
        public async Task<List<Lasku>> HaeMaksamattomatLaskutAsync()
        {
            var lista = new List<Lasku>();
            using var conn = await _db.GetConnectionAsync();
            var cmd = new MySqlCommand("SELECT * FROM lasku WHERE maksettu = 0", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Lasku
                {
                    LaskuId = reader.GetInt32("lasku_id"),
                    VarausId = reader.GetInt32("varaus_id"),
                    Summa = reader.GetDecimal("summa"),
                    Alv = reader.GetDecimal("alv"),
                    Maksettu = reader.GetBoolean("maksettu")
                });
            }

            return lista;
        }

        // Esimerkki: hae raportti mökeittäin
        public async Task<List<(int MokkiId, decimal Kokonaissumma)>> HaeMokkiRaporttiAsync()
        {
            var lista = new List<(int, decimal)>();
            using var conn = await _db.GetConnectionAsync();
            var cmd = new MySqlCommand(
                "SELECT m.mokki_id, SUM(l.summa) AS kokonaissumma " +
                "FROM lasku l JOIN varaus v ON l.varaus_id = v.varaus_id " +
                "JOIN mokki m ON v.mokki_id = m.mokki_id " +
                "GROUP BY m.mokki_id", conn);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add((reader.GetInt32("mokki_id"), reader.GetDecimal("kokonaissumma")));
            }

            return lista;
        }
    }
}
