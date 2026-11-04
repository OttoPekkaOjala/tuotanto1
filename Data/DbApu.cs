using System.Globalization;
using MySqlConnector;

namespace tuotanto1.Data
{
    // Apumetodit tietokannasta lukemiseen ja syötteiden tarkistamiseen
    public static class DbApu
    {
        public static string Teksti(MySqlDataReader r, string sarake)
        {
            int i = r.GetOrdinal(sarake);
            return r.IsDBNull(i) ? "" : Convert.ToString(r.GetValue(i)) ?? "";
        }

        public static int Luku(MySqlDataReader r, string sarake)
        {
            int i = r.GetOrdinal(sarake);
            return r.IsDBNull(i) ? 0 : Convert.ToInt32(r.GetValue(i));
        }

        public static decimal Desimaali(MySqlDataReader r, string sarake)
        {
            int i = r.GetOrdinal(sarake);
            return r.IsDBNull(i) ? 0 : Convert.ToDecimal(r.GetValue(i));
        }

        public static DateTime Pvm(MySqlDataReader r, string sarake)
        {
            int i = r.GetOrdinal(sarake);
            return r.IsDBNull(i) ? DateTime.Today : Convert.ToDateTime(r.GetValue(i));
        }

        // Hakuehto LIKE-lauseeseen. Kanta käyttää utf8mb4_bin-lajittelua,
        // joten haku tehdään pienillä kirjaimilla (SQL:ssä LOWER(sarake)).
        public static string Hakuehto(string? hakusana)
        {
            return "%" + (hakusana ?? "").Trim().ToLower() + "%";
        }

        // Hyväksyy sekä pilkun että pisteen desimaalierottimena
        public static bool OnkoDesimaali(string? syote, out decimal arvo)
        {
            string s = (syote ?? "").Trim().Replace(',', '.');
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out arvo);
        }

        public static bool OnkoPostinumero(string? syote)
        {
            string s = (syote ?? "").Trim();
            return s.Length == 5 && s.All(char.IsDigit);
        }

        // Muuttaa poikkeuksen käyttäjälle näytettäväksi virheilmoitukseksi
        public static string VirheTeksti(Exception ex)
        {
            if (ex is MySqlException mex)
            {
                switch (mex.Number)
                {
                    case 1451:
                        return "Tietoa ei voi poistaa, koska siihen liittyy muita tietoja (esim. mökkejä, palveluita, varauksia tai laskuja).";
                    case 1452:
                        return "Tallennus ei onnistunut, koska viitattua tietoa ei löydy tietokannasta.";
                    case 1042:
                        return "Tietokantaan ei saada yhteyttä. Tarkista, että MySQL on käynnissä.";
                    case 1045:
                        return "Tietokannan käyttäjätunnus tai salasana on väärin.";
                    case 1049:
                        return "Tietokantaa ei löydy. Tarkista tietokannan nimi.";
                    default:
                        return "Tietokantavirhe: " + mex.Message;
                }
            }
            return "Virhe: " + ex.Message;
        }
    }
}
