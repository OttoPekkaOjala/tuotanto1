using MySqlConnector;
using tuotanto1.Data;
using tuotanto1.Models;

namespace tuotanto1.Services
{
    public static class AsiakasService
    {
        // ---------------------------------------------------------
        // HAE ASIAKKAAT (nimellä tai sähköpostilla)
        // ---------------------------------------------------------
        public static List<Asiakas> HaeAsiakkaat(string? hakusana)
        {
            var lista = new List<Asiakas>();

            using var conn = Database.GetConnection();

            string sql = @"SELECT * FROM asiakas
                           WHERE LOWER(IFNULL(etunimi, '')) LIKE @hakusana
                              OR LOWER(IFNULL(sukunimi, '')) LIKE @hakusana
                              OR LOWER(CONCAT(IFNULL(etunimi, ''), ' ', IFNULL(sukunimi, ''))) LIKE @hakusana
                              OR LOWER(IFNULL(email, '')) LIKE @hakusana
                           ORDER BY sukunimi, etunimi";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@hakusana", DbApu.Hakuehto(hakusana));

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Asiakas
                {
                    AsiakasID = DbApu.Luku(reader, "asiakas_id"),
                    Etunimi = DbApu.Teksti(reader, "etunimi"),
                    Sukunimi = DbApu.Teksti(reader, "sukunimi"),
                    Sahkoposti = DbApu.Teksti(reader, "email"),
                    Puhelinnumero = DbApu.Teksti(reader, "puhelinnro"),
                    Lahiosoite = DbApu.Teksti(reader, "lahiosoite"),
                    Postinumero = DbApu.Teksti(reader, "postinro")
                });
            }

            return lista;
        }

        // ---------------------------------------------------------
        // POISTA ASIAKAS
        // ---------------------------------------------------------
        public static void PoistaAsiakas(int id)
        {
            using var conn = Database.GetConnection();

            string sql = "DELETE FROM asiakas WHERE asiakas_id=@id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        // ---------------------------------------------------------
        // LISÄÄ ASIAKAS
        // ---------------------------------------------------------
        public static async Task LisaaAsiakas(Asiakas asiakas)
        {
            using var conn = Database.GetConnection();

            await PostiService.VarmistaPostinumeroAsync(conn, asiakas.Postinumero);

            string sql = @"INSERT INTO asiakas (postinro, etunimi, sukunimi, lahiosoite, email, puhelinnro)
                           VALUES (@postinro, @etunimi, @sukunimi, @lahiosoite, @email, @puhelin)";

            using var cmd = new MySqlCommand(sql, conn);
            LisaaParametrit(cmd, asiakas);
            await cmd.ExecuteNonQueryAsync();
        }

        // ---------------------------------------------------------
        // MUOKKAA ASIAKASTA
        // ---------------------------------------------------------
        public static async Task MuokkaaAsiakas(Asiakas asiakas)
        {
            using var conn = Database.GetConnection();

            await PostiService.VarmistaPostinumeroAsync(conn, asiakas.Postinumero);

            string sql = @"UPDATE asiakas
                           SET postinro=@postinro, etunimi=@etunimi, sukunimi=@sukunimi,
                               lahiosoite=@lahiosoite, email=@email, puhelinnro=@puhelin
                           WHERE asiakas_id=@id";

            using var cmd = new MySqlCommand(sql, conn);
            LisaaParametrit(cmd, asiakas);
            cmd.Parameters.AddWithValue("@id", asiakas.AsiakasID);
            await cmd.ExecuteNonQueryAsync();
        }

        private static void LisaaParametrit(MySqlCommand cmd, Asiakas asiakas)
        {
            cmd.Parameters.AddWithValue("@postinro", asiakas.Postinumero);
            cmd.Parameters.AddWithValue("@etunimi", asiakas.Etunimi);
            cmd.Parameters.AddWithValue("@sukunimi", asiakas.Sukunimi);
            cmd.Parameters.AddWithValue("@lahiosoite", asiakas.Lahiosoite);
            cmd.Parameters.AddWithValue("@email", asiakas.Sahkoposti);
            cmd.Parameters.AddWithValue("@puhelin", asiakas.Puhelinnumero);
        }
    }
}
