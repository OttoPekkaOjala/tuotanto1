using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class AlueService
    {
        public async Task<List<Alue>> HaeAlueetAsync(string? hakusana = "")
        {
            var lista = new List<Alue>();

            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand(
                "SELECT * FROM alue WHERE LOWER(IFNULL(nimi, '')) LIKE @haku ORDER BY nimi", conn);
            cmd.Parameters.AddWithValue("@haku", DbApu.Hakuehto(hakusana));

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Alue
                {
                    AlueId = DbApu.Luku(reader, "alue_id"),
                    Nimi = DbApu.Teksti(reader, "nimi")
                });
            }

            return lista;
        }

        public async Task LisaaAlueAsync(Alue alue)
        {
            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand("INSERT INTO alue (nimi) VALUES (@nimi)", conn);
            cmd.Parameters.AddWithValue("@nimi", alue.Nimi);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task MuokkaaAlueAsync(Alue alue)
        {
            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand("UPDATE alue SET nimi=@nimi WHERE alue_id=@id", conn);
            cmd.Parameters.AddWithValue("@nimi", alue.Nimi);
            cmd.Parameters.AddWithValue("@id", alue.AlueId);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task PoistaAlueAsync(int alueId)
        {
            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand("DELETE FROM alue WHERE alue_id=@id", conn);
            cmd.Parameters.AddWithValue("@id", alueId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
