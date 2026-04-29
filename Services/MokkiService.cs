using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class MokkiService
    {
        private readonly Database _db = new();

        public async Task<List<Mokki>> HaeKaikkiMokitAsync()
        {
            var lista = new List<Mokki>();
            using var conn = await _db.GetConnectionAsync();
            var cmd = new MySqlCommand("SELECT * FROM mokki", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Mokki
                {
                    MokkiId = reader.GetInt32("mokki_id"),
                    AlueId = reader.GetInt32("alue_id"),
                    Nimi = reader.GetString("nimi"),
                    Kuvaus = reader.GetString("kuvaus"),
                    Katuosoite = reader.GetString("katuosoite"),
                    Postinumero = reader.GetString("postinumero"),
                    Henkilomaara = reader.GetInt32("henkilomaara"),
                    Hinta = reader.GetDecimal("hinta")
                });
            }
            return lista;
        }

        public async Task<bool> LisaaMokkiAsync(Mokki mokki)
        {
            using var conn = await _db.GetConnectionAsync();
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