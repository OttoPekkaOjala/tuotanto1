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

            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand("SELECT * FROM posti ORDER BY postinro", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Posti
                {
                    Postinumero = DbApu.Teksti(reader, "postinro"),
                    Toimipaikka = DbApu.Teksti(reader, "toimipaikka")
                });
            }

            return lista;
        }

        // Asiakas- ja mokki-taulut viittaavat posti-tauluun, joten postinumeron
        // täytyy löytyä sieltä ennen tallennusta. Puuttuva numero lisätään.
        public static async Task VarmistaPostinumeroAsync(MySqlConnection conn, string postinro)
        {
            using var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM posti WHERE postinro = @postinro", conn);
            checkCmd.Parameters.AddWithValue("@postinro", postinro);
            long maara = Convert.ToInt64(await checkCmd.ExecuteScalarAsync());

            if (maara == 0)
            {
                using var insertCmd = new MySqlCommand(
                    "INSERT INTO posti (postinro, toimipaikka) VALUES (@postinro, 'Tuntematon')", conn);
                insertCmd.Parameters.AddWithValue("@postinro", postinro);
                await insertCmd.ExecuteNonQueryAsync();
            }
        }
    }
}
