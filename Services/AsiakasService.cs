using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class AsiakasService
    {
        private readonly Database _db = new();

        public async Task<List<Asiakas>> HaeKaikkiAsiakkaatAsync()
        {
            var lista = new List<Asiakas>();
            using var conn = await _db.GetConnectionAsync();
            var cmd = new MySqlCommand("SELECT * FROM asiakas", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Asiakas
                {
                    AsiakasId = reader.GetInt32("asiakas_id"),
                    Etunimi = reader.GetString("etunimi"),
                    Sukunimi = reader.GetString("sukunimi"),
                    Lahiosoite = reader.GetString("lahiosoite"),
                    Postinumero = reader.GetString("postinumero"),
                    Puhelinnumero = reader.GetString("puhelinnumero"),
                    Email = reader.GetString("email")
                });
            }
            return lista;
        }

        public async Task<bool> LisaaAsiakasAsync(Asiakas asiakas)
        {
            using var conn = await _db.GetConnectionAsync();
            var cmd = new MySqlCommand(
                "INSERT INTO asiakas (etunimi, sukunimi, lahiosoite, postinumero, puhelinnumero, email) " +
                "VALUES (@etunimi, @sukunimi, @lahiosoite, @postinumero, @puhelinnumero, @email)", conn);

            cmd.Parameters.AddWithValue("@etunimi", asiakas.Etunimi);
            cmd.Parameters.AddWithValue("@sukunimi", asiakas.Sukunimi);
            cmd.Parameters.AddWithValue("@lahiosoite", asiakas.Lahiosoite);
            cmd.Parameters.AddWithValue("@postinumero", asiakas.Postinumero);
            cmd.Parameters.AddWithValue("@puhelinnumero", asiakas.Puhelinnumero);
            cmd.Parameters.AddWithValue("@email", asiakas.Email);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}