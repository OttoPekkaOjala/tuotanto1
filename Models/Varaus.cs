namespace tuotanto1.Models
{
    public class Varaus
    {
        public int VarausId { get; set; }
        public int AsiakasId { get; set; }
        public int MokkiId { get; set; }
        public DateTime VarattuAlkupvm { get; set; }
        public DateTime VarattuLoppupvm { get; set; }
    }
}
