using MySqlConnector;
using tuotanto1.Data;
using tuotanto1.Models;

namespace tuotanto1.Services
{
    public static class AsiakasService
    {
        // ---------------------------------------------------------
        // HAE ASIAKKAAT
        // ---------------------------------------------------------
        public static List<Asiakas> HaeAsiakkaat(string hakusana)
        {
            var lista = new List<Asiakas>();

            using var conn = Database.GetConnection();

            string sql = @"SELECT * FROM asiakas
                           WHERE etunimi LIKE @hakusana
                           OR sukunimi LIKE @hakusana";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@hakusana", $"%{hakusana}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Asiakas
                {
                    AsiakasID = reader.GetInt32(reader.GetOrdinal("asiakas_id")),

                    Etunimi = reader.IsDBNull(reader.GetOrdinal("etunimi"))
                        ? ""
                        : reader.GetString(reader.GetOrdinal("etunimi")),

                    Sukunimi = reader.IsDBNull(reader.GetOrdinal("sukunimi"))
                        ? ""
                        : reader.GetString(reader.GetOrdinal("sukunimi")),

                    Sahkoposti = reader.IsDBNull(reader.GetOrdinal("email"))
                        ? ""
                        : reader.GetString(reader.GetOrdinal("email")),

                    Puhelinnumero = reader.IsDBNull(reader.GetOrdinal("puhelinnro"))
                        ? ""
                        : reader.GetString(reader.GetOrdinal("puhelinnro")),

                    Lahiosoite = reader.IsDBNull(reader.GetOrdinal("lahiosoite"))
                        ? ""
                        : reader.GetString(reader.GetOrdinal("lahiosoite")),

                    Postinumero = reader.IsDBNull(reader.GetOrdinal("postinro"))
                        ? ""
                        : reader.GetString(reader.GetOrdinal("postinro"))
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

            // 1. Tarkista löytyykö postinumero posti-taulusta
            string checkSql = "SELECT COUNT(*) FROM posti WHERE postinro = @postinro";
            using (var checkCmd = new MySqlCommand(checkSql, conn))
            {
                checkCmd.Parameters.AddWithValue("@postinro", asiakas.Postinumero);
                long count = (long)await checkCmd.ExecuteScalarAsync();

                if (count == 0)
                {
                    // Lisää postinumero automaattisesti
                    string insertPostiSql =
                        "INSERT INTO posti (postinro, toimipaikka) VALUES (@postinro, 'Tuntematon')";

                    using var insertPostiCmd = new MySqlCommand(insertPostiSql, conn);
                    insertPostiCmd.Parameters.AddWithValue("@postinro", asiakas.Postinumero);
                    await insertPostiCmd.ExecuteNonQueryAsync();
                }
            }

            // 2. Lisää asiakas
            string sql = @"INSERT INTO asiakas (postinro, etunimi, sukunimi, email, puhelinnro)
                           VALUES (@postinro, @etunimi, @sukunimi, @email, @puhelin)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@postinro", asiakas.Postinumero);
            cmd.Parameters.AddWithValue("@etunimi", asiakas.Etunimi);
            cmd.Parameters.AddWithValue("@sukunimi", asiakas.Sukunimi);
            cmd.Parameters.AddWithValue("@email", asiakas.Sahkoposti);
            cmd.Parameters.AddWithValue("@puhelin", asiakas.Puhelinnumero);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}





