using System.Collections.ObjectModel;
using tuotanto1.Models;
using tuotanto1.Services;

namespace tuotanto1.ViewModels
{
    public class VarausViewModel
    {
        public ObservableCollection<Varaus> Varaukset { get; set; } = new();
        private readonly VarausService _service = new();

        public async Task LataaVarauksetAsync()
        {
            var lista = await _service.HaeKaikkiVarauksetAsync();
            Varaukset.Clear();

            foreach (var v in lista)
                Varaukset.Add(v);
        }

        public async Task<bool> LisaaVarausAsync(Varaus varaus)
        {
            return await _service.LisaaVarausAsync(varaus);
        }
    }
}
