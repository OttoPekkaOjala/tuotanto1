using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class PalveluService
    {
        public Task<List<Palvelu>> HaeKaikkiPalvelutAsync()
        {
            return HaePalvelutAsync("");
        }

        // Haku palvelun tai alueen nimellä
        public async Task<List<Palvelu>> HaePalvelutAsync(string? hakusana)
        {
            var lista = new List<Palvelu>();

            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand(
                @"SELECT p.*, a.nimi AS alue_nimi
                  FROM palvelu p
                  LEFT JOIN alue a ON a.alue_id = p.alue_id
                  WHERE LOWER(IFNULL(p.nimi, '')) LIKE @haku
                     OR LOWER(IFNULL(a.nimi, '')) LIKE @haku
                  ORDER BY p.nimi", conn);
            cmd.Parameters.AddWithValue("@haku", DbApu.Hakuehto(hakusana));

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Palvelu
                {
                    PalveluId = DbApu.Luku(reader, "palvelu_id"),
                    AlueId = DbApu.Luku(reader, "alue_id"),
                    Nimi = DbApu.Teksti(reader, "nimi"),
                    Kuvaus = DbApu.Teksti(reader, "kuvaus"),
                    Hinta = DbApu.Desimaali(reader, "hinta"),
                    Alv = DbApu.Desimaali(reader, "alv"),
                    AlueNimi = DbApu.Teksti(reader, "alue_nimi")
                });
            }

            return lista;
        }

        public async Task LisaaPalveluAsync(Palvelu palvelu)
        {
            using var conn = Database.GetConnection();

            // palvelu_id ei ole AUTO_INCREMENT, joten seuraava id lasketaan itse
            using var idCmd = new MySqlCommand("SELECT IFNULL(MAX(palvelu_id), 0) + 1 FROM palvelu", conn);
            int uusiId = Convert.ToInt32(await idCmd.ExecuteScalarAsync());

            using var cmd = new MySqlCommand(
                "INSERT INTO palvelu (palvelu_id, alue_id, nimi, kuvaus, hinta, alv) " +
                "VALUES (@id, @alue_id, @nimi, @kuvaus, @hinta, @alv)", conn);
            cmd.Parameters.AddWithValue("@id", uusiId);
            LisaaParametrit(cmd, palvelu);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task MuokkaaPalveluAsync(Palvelu palvelu)
        {
            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand(
                "UPDATE palvelu SET alue_id=@alue_id, nimi=@nimi, kuvaus=@kuvaus, hinta=@hinta, alv=@alv " +
                "WHERE palvelu_id=@id", conn);
            cmd.Parameters.AddWithValue("@id", palvelu.PalveluId);
            LisaaParametrit(cmd, palvelu);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task PoistaPalveluAsync(int palveluId)
        {
            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand("DELETE FROM palvelu WHERE palvelu_id=@id", conn);
            cmd.Parameters.AddWithValue("@id", palveluId);
            await cmd.ExecuteNonQueryAsync();
        }

        private static void LisaaParametrit(MySqlCommand cmd, Palvelu palvelu)
        {
            cmd.Parameters.AddWithValue("@alue_id", palvelu.AlueId);
            cmd.Parameters.AddWithValue("@nimi", palvelu.Nimi);
            cmd.Parameters.AddWithValue("@kuvaus", palvelu.Kuvaus);
            cmd.Parameters.AddWithValue("@hinta", palvelu.Hinta);
            cmd.Parameters.AddWithValue("@alv", palvelu.Alv);
        }
    }
}
