namespace tuotanto1.Models
{
    public class Varaus
    {
        public int VarausId { get; set; }
        public int AsiakasId { get; set; }
        public int MokkiId { get; set; }
        public DateTime VarattuAlkupvm { get; set; }
        public DateTime VarattuLoppupvm { get; set; }

        // Näytetään listassa, haetaan asiakas- ja mokki-tauluista
        public string AsiakasNimi { get; set; } = "";
        public string MokkiNimi { get; set; } = "";

        public string Otsikko => $"{AsiakasNimi} – {MokkiNimi}";
        public string Ajankohta => $"{VarattuAlkupvm:d.M.yyyy} – {VarattuLoppupvm:d.M.yyyy}";
    }
}
