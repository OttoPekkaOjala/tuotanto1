using MySql.Data.MySqlClient;
using MySqlConnector;
using tuotanto1.Models;

namespace tuotanto1.Data
{
    public static class Database
    {
        private static string connectionString =
            "Server=localhost;Database=vuokraamo;Uid=root;Pwd=;SslMode=none;";

        public static List<Asiakas> HaeAsiakkaat()
        {
            var lista = new List<Asiakas>();

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string sql = "SELECT * FROM asiakas";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Asiakas
                {
                    AsiakasId = reader.GetInt32("asiakas_id"),
                    Etunimi = reader.GetString("etunimi"),
                    Sukunimi = reader.GetString("sukunimi"),
                    Lahiosoite = reader.GetString("lahiosoite"),
                    Postinumero = reader.GetString("postinumero"),
                    Email = reader.GetString("email"),
                    Puhelinnumero = reader.GetString("puhelinnumero")
                });
            }

            return lista;
        }

        public static void LisaaAsiakas(Asiakas a)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string sql = @"INSERT INTO asiakas 
                           (etunimi, sukunimi, lahiosoite, postinumero, email, puhelinnumero)
                           VALUES (@etunimi, @sukunimi, @lahiosoite, @postinumero, @email, @puhelinnumero)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@etunimi", a.Etunimi);
            cmd.Parameters.AddWithValue("@sukunimi", a.Sukunimi);
            cmd.Parameters.AddWithValue("@lahiosoite", a.Lahiosoite);
            cmd.Parameters.AddWithValue("@postinumero", a.Postinumero);
            cmd.Parameters.AddWithValue("@email", a.Email);
            cmd.Parameters.AddWithValue("@puhelinnumero", a.Puhelinnumero);

            cmd.ExecuteNonQuery();
        }
    }
}
