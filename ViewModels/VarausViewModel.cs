using System;
using System.Collections.ObjectModel;
using tuotanto1.Models;

namespace tuotanto1.ViewModels
{
    public class VarausViewModel
    {
        public ObservableCollection<Varaus> Varaukset { get; set; }

        public VarausViewModel()
        {
            Varaukset = new ObservableCollection<Varaus>
            {
                new Varaus
                {
                    VarausId = 1,
                    AsiakasId = 1,
                    MokkiId = 3,
                    Alkupvm = new DateTime(2024, 6, 10),
                    Loppupvm = new DateTime(2024, 6, 15),
                    Hinta = 550.00m
                },
                new Varaus
                {
                    VarausId = 2,
                    AsiakasId = 2,
                    MokkiId = 1,
                    Alkupvm = new DateTime(2024, 7, 1),
                    Loppupvm = new DateTime(2024, 7, 5),
                    Hinta = 420.00m
                },
                new Varaus
                {
                    VarausId = 3,
                    AsiakasId = 1,
                    MokkiId = 2,
                    Alkupvm = new DateTime(2024, 8, 12),
                    Loppupvm = new DateTime(2024, 8, 20),
                    Hinta = 890.00m
                }
            };
        }
    }
}
