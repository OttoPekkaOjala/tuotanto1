using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tuotanto1.Models;

public class Varaus
{
    public int VarausId { get; set; }
    public int AsiakasId { get; set; }
    public int MokkiId { get; set; }
    public DateTime VarattuPvm { get; set; }
    public DateTime VahvistusPvm { get; set; }
    public DateTime VarattuAlkuPvm { get; set; }
    public DateTime VarattuLoppuPvm { get; set; }
}