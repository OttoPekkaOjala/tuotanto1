using tuotanto1.Data;
using tuotanto1.Models;
using MySqlConnector;

namespace tuotanto1.Services
{
    public class VarausService
    {
        private readonly Database _db = new();

        public async Task<List<Varaus>> HaeKaikkiVarauksetAsync()
        {
            var lista = new List<Varaus>();
            using var conn = await _db.GetConnectionAsync();
            var cmd = new MySqlCommand("SELECT * FROM varaus", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Varaus
                {
                    VarausId = reader.GetInt32("varaus_id"),
                    AsiakasId = reader.GetInt32("asiakas_id"),
                    MokkiId = reader.GetInt32("mokki_id"),
                    VarattuAlkupvm = reader.GetDateTime("varattu_alkupvm"),
                    VarattuLoppupvm = reader.GetDateTime("varattu_loppupvm")
                });
            }
            return lista;
        }

        public async Task<bool> LisaaVarausAsync(Varaus varaus)
        {
            using var conn = await _db.GetConnectionAsync();
            var cmd = new MySqlCommand(
                "INSERT INTO varaus (asiakas_id, mokki_id, varattu_alkupvm, varattu_loppupvm) VALUES (@asiakas_id, @mokki_id, @alku, @loppu)", conn);
            cmd.Parameters.AddWithValue("@asiakas_id", varaus.AsiakasId);
            cmd.Parameters.AddWithValue("@mokki_id", varaus.MokkiId);
            cmd.Parameters.AddWithValue("@alku", varaus.VarattuAlkupvm);
            cmd.Parameters.AddWithValue("@loppu", varaus.VarattuLoppupvm);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}