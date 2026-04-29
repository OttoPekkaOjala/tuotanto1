using System.Collections.ObjectModel;
using tuotanto1.Models;
using tuotanto1.Services;

namespace tuotanto1.ViewModels
{
    public class LaskuViewModel
    {
        public ObservableCollection<Lasku> Laskut { get; set; } = new();
        private readonly LaskutusService _service = new();

        public async Task LataaLaskutAsync()
        {
            var lista = await _service.HaeKaikkiLaskutAsync();
            Laskut.Clear();

            foreach (var l in lista)
                Laskut.Add(l);
        }

        public async Task<bool> LisaaLaskuAsync(Lasku lasku)
        {
            return await _service.LisaaLaskuAsync(lasku);
        }
    }
}