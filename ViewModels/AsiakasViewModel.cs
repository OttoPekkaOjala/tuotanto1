using System.Collections.ObjectModel;
using tuotanto1.Models;
using tuotanto1.Services;


namespace tuotanto1.ViewModels
{
    public class AsiakasViewModel
    {
        public ObservableCollection<Asiakas> Asiakkaat { get; set; } = new();
        private readonly AsiakasService _service = new();

        public async Task LataaAsiakkaatAsync()
        {
            var lista = await _service.HaeKaikkiAsiakkaatAsync();
            Asiakkaat.Clear();

            foreach (var a in lista)
                Asiakkaat.Add(a);
        }

        public async Task<bool> LisaaAsiakasAsync(Asiakas asiakas)
        {
            return await _service.LisaaAsiakasAsync(asiakas);
        }
    }
}