using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tuotanto1.Models;

public class Palvelu
{
    public int PalveluId { get; set; }
    public int AlueId { get; set; }
    public string Nimi { get; set; }
    public string Kuvaus { get; set; }
    public double Hinta { get; set; }
    public double Alv { get; set; }
}