namespace tuotanto1.Models;

public class Asiakas
{
    public int AsiakasID { get; set; }
    public string Etunimi { get; set; } = "";
    public string Sukunimi { get; set; } = "";
    public string Lahiosoite { get; set; } = "";
    public string Sahkoposti { get; set; } = "";
    public string Puhelinnumero { get; set; } = "";
    public string Postinumero { get; set; } = "";

    // Tämä on automaattinen yhdistelmäkenttä
    public string Nimi => $"{Etunimi} {Sukunimi}";

    public override string ToString() => Nimi;
}
