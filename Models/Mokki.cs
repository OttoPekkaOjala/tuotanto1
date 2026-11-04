namespace tuotanto1.Models
{
    public class Mokki
    {
        public int MokkiId { get; set; }
        public int AlueId { get; set; }
        public string Nimi { get; set; } = "";          // sarake mokkinimi
        public string Kuvaus { get; set; } = "";
        public string Katuosoite { get; set; } = "";
        public string Postinumero { get; set; } = "";   // sarake postinro
        public int Henkilomaara { get; set; }
        public decimal Hinta { get; set; }
        public string Varustelu { get; set; } = "";

        // Näytetään listassa, haetaan alue-taulusta
        public string AlueNimi { get; set; } = "";

        public string Sijainti => $"{Katuosoite}, {Postinumero}";
        public string Lisatiedot => $"{AlueNimi} | {Sijainti} | {Hinta:0.00} € | {Henkilomaara} hlö";

        public override string ToString() => Nimi;
    }
}
