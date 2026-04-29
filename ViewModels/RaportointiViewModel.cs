using System.Collections.ObjectModel;
using tuotanto1.Models;
using tuotanto1.Services;

namespace tuotanto1.ViewModels
{
    public class RaportointiViewModel
    {
        private readonly LaskutusService _laskutusService = new();

        public ObservableCollection<Lasku> Laskut { get; set; }
        public decimal Kokonaissumma { get; set; }

        public RaportointiViewModel()
        {
            Laskut = new ObservableCollection<Lasku>();
        }

        // Hae kaikki laskut ja laske kokonaissumma
        public async Task HaeKaikkiLaskutAsync()
        {
            var lista = await _laskutusService.HaeKaikkiLaskutAsync();
            Laskut.Clear();

            decimal summa = 0;
            foreach (var lasku in lista)
            {
                Laskut.Add(lasku);
                summa += lasku.Summa;
            }

            Kokonaissumma = summa;
        }

        // Hae vain maksamattomat laskut
        public async Task HaeMaksamattomatLaskutAsync()
        {
            var lista = await _laskutusService.HaeKaikkiLaskutAsync();
            Laskut.Clear();

            foreach (var lasku in lista.Where(l => !l.Maksettu))
                Laskut.Add(lasku);
        }

        // 🔧 Tämä metodi korjaa virheen RaportitPage.xaml.cs:ssä
        public async Task LataaRaportitAsync()
        {
            await HaeKaikkiLaskutAsync();
        }
    }
}
