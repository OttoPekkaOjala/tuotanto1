using System.Collections.ObjectModel;
using tuotanto1.Models;

namespace tuotanto1.ViewModels;

public class MokkiViewModel
{
    public ObservableCollection<Mokki> Mokit { get; set; }

    public MokkiViewModel()
    {
        Mokit = new ObservableCollection<Mokki>
        {
            new Mokki
            {
                MokkiId = 1,
                AlueId = 1,
                Postinro = "70100",
                Mokkinimi = "Hirsihuvila",
                Katuosoite = "Metsäpolku 5",
                Hinta = 120,
                Kuvaus = "Tunnelmallinen hirsimökki järven rannalla",
                Henkilomaara = 4,
                Varustelu = "Sauna, takka, wifi"
            },
            new Mokki
            {
                MokkiId = 2,
                AlueId = 1,
                Postinro = "70100",
                Mokkinimi = "Rantamökki",
                Katuosoite = "Rantatie 12",
                Hinta = 150,
                Kuvaus = "Rantamökki omalla laiturilla",
                Henkilomaara = 6,
                Varustelu = "Sauna, vene, grillikatos"
            }
        };
    }
}