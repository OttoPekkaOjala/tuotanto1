using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tuotanto1.Models;

namespace tuotanto1.Models;

public class Asiakas
{
    public int AsiakasID { get; set; }
    public string Etunimi { get; set; }
    public string Sukunimi { get; set; }
    public string Lahiosoite { get; set; }
    public string Sahkoposti { get; set; }
    public string Puhelinnumero { get; set; }
    public string Postinumero { get; set; }

    // Tämä on automaattinen yhdistelmäkenttä
    public string Nimi => $"{Etunimi} {Sukunimi}";
}