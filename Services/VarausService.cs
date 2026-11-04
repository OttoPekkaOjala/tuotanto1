using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class VarausService
    {
        public Task<List<Varaus>> HaeKaikkiVarauksetAsync()
        {
            return HaeVarauksetAsync("");
        }

        // Haku asiakkaan tai mökin nimellä
        public async Task<List<Varaus>> HaeVarauksetAsync(string? hakusana)
        {
            var lista = new List<Varaus>();

            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand(
                @"SELECT v.*,
                         CONCAT(IFNULL(a.etunimi, ''), ' ', IFNULL(a.sukunimi, '')) AS asiakas_nimi,
                         m.mokkinimi AS mokki_nimi
                  FROM varaus v
                  LEFT JOIN asiakas a ON a.asiakas_id = v.asiakas_id
                  LEFT JOIN mokki m ON m.mokki_id = v.mokki_id
                  WHERE LOWER(CONCAT(IFNULL(a.etunimi, ''), ' ', IFNULL(a.sukunimi, ''))) LIKE @haku
                     OR LOWER(IFNULL(m.mokkinimi, '')) LIKE @haku
                  ORDER BY v.varattu_alkupvm DESC", conn);
            cmd.Parameters.AddWithValue("@haku", DbApu.Hakuehto(hakusana));

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Varaus
                {
                    VarausId = DbApu.Luku(reader, "varaus_id"),
                    AsiakasId = DbApu.Luku(reader, "asiakas_id"),
                    MokkiId = DbApu.Luku(reader, "mokki_id"),
                    VarattuAlkupvm = DbApu.Pvm(reader, "varattu_alkupvm"),
                    VarattuLoppupvm = DbApu.Pvm(reader, "varattu_loppupvm"),
                    AsiakasNimi = DbApu.Teksti(reader, "asiakas_nimi"),
                    MokkiNimi = DbApu.Teksti(reader, "mokki_nimi")
                });
            }

            return lista;
        }

        // Onko mökillä jo varaus, joka menee päällekkäin annetun ajan kanssa.
        // Muokattaessa varaus itse jätetään tarkistuksen ulkopuolelle.
        public async Task<bool> OnkoPaallekkainenAsync(int mokkiId, DateTime alku, DateTime loppu, int ohitaVarausId)
        {
            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand(
                @"SELECT COUNT(*) FROM varaus
                  WHERE mokki_id = @mokki
                    AND varaus_id <> @ohita
                    AND varattu_alkupvm < @loppu
                    AND varattu_loppupvm > @alku", conn);
            cmd.Parameters.AddWithValue("@mokki", mokkiId);
            cmd.Parameters.AddWithValue("@ohita", ohitaVarausId);
            cmd.Parameters.AddWithValue("@alku", alku);
            cmd.Parameters.AddWithValue("@loppu", loppu);

            return Convert.ToInt64(await cmd.ExecuteScalarAsync()) > 0;
        }

        public async Task<bool> LisaaVarausAsync(Varaus varaus)
        {
            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand(
                "INSERT INTO varaus (asiakas_id, mokki_id, varattu_pvm, vahvistus_pvm, varattu_alkupvm, varattu_loppupvm) " +
                "VALUES (@asiakas_id, @mokki_id, NOW(), NOW(), @alku, @loppu)",
                conn);
            LisaaParametrit(cmd, varaus);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task MuokkaaVarausAsync(Varaus varaus)
        {
            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand(
                "UPDATE varaus SET asiakas_id=@asiakas_id, mokki_id=@mokki_id, " +
                "varattu_alkupvm=@alku, varattu_loppupvm=@loppu WHERE varaus_id=@id", conn);
            LisaaParametrit(cmd, varaus);
            cmd.Parameters.AddWithValue("@id", varaus.VarausId);
            await cmd.ExecuteNonQueryAsync();
        }

        // Poistaa ensin varaukseen liitetyt palvelut ja sitten varauksen.
        // Jos varauksella on lasku, kanta estää poiston (virhe 1451).
        public async Task PoistaVarausAsync(int varausId)
        {
            using var conn = Database.GetConnection();
            using var tx = await conn.BeginTransactionAsync();

            using (var vpCmd = new MySqlCommand("DELETE FROM varauksen_palvelut WHERE varaus_id=@id", conn, tx))
            {
                vpCmd.Parameters.AddWithValue("@id", varausId);
                await vpCmd.ExecuteNonQueryAsync();
            }

            using (var cmd = new MySqlCommand("DELETE FROM varaus WHERE varaus_id=@id", conn, tx))
            {
                cmd.Parameters.AddWithValue("@id", varausId);
                await cmd.ExecuteNonQueryAsync();
            }

            await tx.CommitAsync();
        }

        private static void LisaaParametrit(MySqlCommand cmd, Varaus varaus)
        {
            cmd.Parameters.AddWithValue("@asiakas_id", varaus.AsiakasId);
            cmd.Parameters.AddWithValue("@mokki_id", varaus.MokkiId);
            cmd.Parameters.AddWithValue("@alku", varaus.VarattuAlkupvm);
            cmd.Parameters.AddWithValue("@loppu", varaus.VarattuLoppupvm);
        }
    }
}
