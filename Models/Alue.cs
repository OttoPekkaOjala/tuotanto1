namespace tuotanto1.Models;

public class Alue
{
    public int AlueId { get; set; }
    public string Nimi { get; set; } = "";

    public override string ToString() => Nimi;
}
