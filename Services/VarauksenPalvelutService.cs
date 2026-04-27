using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class VarauksenPalvelutService
    {
        private readonly Database _db = new();

        public async Task<List<VarauksenPalvelut>> HaeVarauksenPalvelutAsync(int varausId)
        {
            var lista = new List<VarauksenPalvelut>();
            using var conn = await _db.GetConnectionAsync();
            var cmd = new MySqlCommand(
                "SELECT * FROM varauksen_palvelut WHERE varaus_id = @id", conn);

            cmd.Parameters.AddWithValue("@id", varausId);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new VarauksenPalvelut
                {
                    VarausId = reader.GetInt32("varaus_id"),
                    PalveluId = reader.GetInt32("palvelu_id"),
                    Lkm = reader.GetInt32("lkm")
                });
            }
            return lista;
        }
    }
}