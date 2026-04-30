using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;


namespace tuotanto1.Services
{
    public class MokkiService
    {
        public async Task<List<Mokki>> HaeKaikkiMokitAsync()
        {
            var lista = new List<Mokki>();

            using var conn = Database.GetConnection(); // static call
            var cmd = new MySqlCommand("SELECT * FROM mokki", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Mokki
                {
                    MokkiId = reader.GetInt32(reader.GetOrdinal("mokki_id")),
                    AlueId = reader.GetInt32(reader.GetOrdinal("alue_id")),
                    Nimi = reader.GetString(reader.GetOrdinal("nimi")),
                    Kuvaus = reader.GetString(reader.GetOrdinal("kuvaus")),
                    Katuosoite = reader.GetString(reader.GetOrdinal("katuosoite")),
                    Postinumero = reader.GetString(reader.GetOrdinal("postinumero")),
                    Henkilomaara = reader.GetInt32(reader.GetOrdinal("henkilomaara")),
                    Hinta = reader.GetDecimal(reader.GetOrdinal("hinta"))
                });
            }

            return lista;
        }

        public async Task<bool> LisaaMokkiAsync(Mokki mokki)
        {
            using var conn = Database.GetConnection(); // static call

            var cmd = new MySqlCommand(
                "INSERT INTO mokki (alue_id, nimi, kuvaus, katuosoite, postinumero, henkilomaara, hinta) " +
                "VALUES (@alue_id, @nimi, @kuvaus, @katuosoite, @postinumero, @henkilomaara, @hinta)", conn);

            cmd.Parameters.AddWithValue("@alue_id", mokki.AlueId);
            cmd.Parameters.AddWithValue("@nimi", mokki.Nimi);
            cmd.Parameters.AddWithValue("@kuvaus", mokki.Kuvaus);
            cmd.Parameters.AddWithValue("@katuosoite", mokki.Katuosoite);
            cmd.Parameters.AddWithValue("@postinumero", mokki.Postinumero);
            cmd.Parameters.AddWithValue("@henkilomaara", mokki.Henkilomaara);
            cmd.Parameters.AddWithValue("@hinta", mokki.Hinta);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}
