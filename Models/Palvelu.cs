namespace tuotanto1.Models;

public class Palvelu
{
    public int PalveluId { get; set; }
    public int AlueId { get; set; }
    public string Nimi { get; set; } = "";
    public string Kuvaus { get; set; } = "";
    public decimal Hinta { get; set; }
    public decimal Alv { get; set; }

    // Näytetään listassa, haetaan alue-taulusta
    public string AlueNimi { get; set; } = "";

    public string Lisatiedot => $"{AlueNimi} | {Hinta:0.00} € (alv {Alv:0.##} %)";

    public override string ToString() => Nimi;
}
