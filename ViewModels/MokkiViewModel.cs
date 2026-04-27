using System.Collections.ObjectModel;
using tuotanto1.Models;
using tuotanto1.Services;



namespace tuotanto1.ViewModels
{
    public class MokkiViewModel
    {
        public ObservableCollection<Mokki> Mokit { get; set; } = new();
        private readonly MokkiService _service = new();

        public async Task LataaMokitAsync()
        {
            var lista = await _service.HaeKaikkiMokitAsync();
            Mokit.Clear();

            foreach (var m in lista)
                Mokit.Add(m);
        }

        public async Task<bool> LisaaMokkiAsync(Mokki mokki)
        {
            return await _service.LisaaMokkiAsync(mokki);
        }
    }
}