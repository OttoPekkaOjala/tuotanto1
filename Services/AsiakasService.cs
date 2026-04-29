using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tuotanto1.Services;

using Microsoft.Maui.Controls.Shapes;
using tuotanto1.Data;
using tuotanto1.Models;

    public static class AsiakasService
    {
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

        public static void PoistaAsiakas(int id)
        {
            using var conn = Database.GetConnection();
            string sql = "DELETE FROM asiakas WHERE asiakas_id=@id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    public static async Task LisaaAsiakas(Asiakas asiakas)
    {
        using var conn = Database.GetConnection();  // yhteys on jo auki

        // 1. Tarkista löytyykö postinumero
        string checkSql = "SELECT COUNT(*) FROM posti WHERE postinro = @postinro";
        using (var checkCmd = new MySqlCommand(checkSql, conn))
        {
            checkCmd.Parameters.AddWithValue("@postinro", asiakas.Postinumero);
            long count = (long)await checkCmd.ExecuteScalarAsync();

            if (count == 0)
            {
                string insertPostiSql = "INSERT INTO posti (postinro, toimipaikka) VALUES (@postinro, 'Tuntematon')";
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

