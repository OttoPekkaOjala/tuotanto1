namespace tuotanto1.Models
{
    public class Varaus
    {
        public int VarausId { get; set; }
        public int AsiakasId { get; set; }
        public int MokkiId { get; set; }
        public DateTime Alkupvm { get; set; }
        public DateTime Loppupvm { get; set; }
        public decimal Hinta { get; set; }
    }
}
