using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class MokkiService
    {
        public Task<List<Mokki>> HaeKaikkiMokitAsync()
        {
            return HaeMokitAsync("");
        }

        // Haku mökin nimellä, alueen nimellä tai osoitteella
        public async Task<List<Mokki>> HaeMokitAsync(string? hakusana)
        {
            var lista = new List<Mokki>();

            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand(
                @"SELECT m.*, a.nimi AS alue_nimi
                  FROM mokki m
                  LEFT JOIN alue a ON a.alue_id = m.alue_id
                  WHERE LOWER(IFNULL(m.mokkinimi, '')) LIKE @haku
                     OR LOWER(IFNULL(a.nimi, '')) LIKE @haku
                     OR LOWER(IFNULL(m.katuosoite, '')) LIKE @haku
                  ORDER BY m.mokkinimi", conn);
            cmd.Parameters.AddWithValue("@haku", DbApu.Hakuehto(hakusana));

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Mokki
                {
                    MokkiId = DbApu.Luku(reader, "mokki_id"),
                    AlueId = DbApu.Luku(reader, "alue_id"),
                    Nimi = DbApu.Teksti(reader, "mokkinimi"),
                    Kuvaus = DbApu.Teksti(reader, "kuvaus"),
                    Katuosoite = DbApu.Teksti(reader, "katuosoite"),
                    Postinumero = DbApu.Teksti(reader, "postinro"),
                    Henkilomaara = DbApu.Luku(reader, "henkilomaara"),
                    Hinta = DbApu.Desimaali(reader, "hinta"),
                    Varustelu = DbApu.Teksti(reader, "varustelu"),
                    AlueNimi = DbApu.Teksti(reader, "alue_nimi")
                });
            }

            return lista;
        }

        public async Task<bool> LisaaMokkiAsync(Mokki mokki)
        {
            using var conn = Database.GetConnection();
            await PostiService.VarmistaPostinumeroAsync(conn, mokki.Postinumero);

            using var cmd = new MySqlCommand(
                "INSERT INTO mokki (alue_id, postinro, mokkinimi, katuosoite, hinta, kuvaus, henkilomaara, varustelu) " +
                "VALUES (@alue_id, @postinro, @nimi, @katuosoite, @hinta, @kuvaus, @henkilomaara, @varustelu)", conn);
            LisaaParametrit(cmd, mokki);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> MuokkaaMokkiAsync(Mokki mokki)
        {
            using var conn = Database.GetConnection();
            await PostiService.VarmistaPostinumeroAsync(conn, mokki.Postinumero);

            using var cmd = new MySqlCommand(
                "UPDATE mokki SET alue_id=@alue_id, postinro=@postinro, mokkinimi=@nimi, katuosoite=@katuosoite, " +
                "hinta=@hinta, kuvaus=@kuvaus, henkilomaara=@henkilomaara, varustelu=@varustelu " +
                "WHERE mokki_id=@id", conn);
            LisaaParametrit(cmd, mokki);
            cmd.Parameters.AddWithValue("@id", mokki.MokkiId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task PoistaMokkiAsync(int mokkiId)
        {
            using var conn = Database.GetConnection();
            using var cmd = new MySqlCommand("DELETE FROM mokki WHERE mokki_id=@id", conn);
            cmd.Parameters.AddWithValue("@id", mokkiId);
            await cmd.ExecuteNonQueryAsync();
        }

        private static void LisaaParametrit(MySqlCommand cmd, Mokki mokki)
        {
            cmd.Parameters.AddWithValue("@alue_id", mokki.AlueId);
            cmd.Parameters.AddWithValue("@postinro", mokki.Postinumero);
            cmd.Parameters.AddWithValue("@nimi", mokki.Nimi);
            cmd.Parameters.AddWithValue("@katuosoite", mokki.Katuosoite);
            cmd.Parameters.AddWithValue("@hinta", mokki.Hinta);
            cmd.Parameters.AddWithValue("@kuvaus", mokki.Kuvaus);
            cmd.Parameters.AddWithValue("@henkilomaara", mokki.Henkilomaara);
            cmd.Parameters.AddWithValue("@varustelu", mokki.Varustelu);
        }
    }
}
