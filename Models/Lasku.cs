using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tuotanto1.Models
{
    public class Lasku
    {
        public int LaskuId { get; set; }
        public int VarausId { get; set; }
        public decimal Summa { get; set; }
        public decimal Alv { get; set; }
        public bool Maksettu { get; set; }
    }
}
