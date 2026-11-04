using tuotanto1.Data;
using tuotanto1.Models;
using tuotanto1.Services;

namespace tuotanto1.Views
{
    public partial class VarausPage : ContentPage
    {
        private readonly VarausService _service = new();
        private readonly MokkiService _mokkiService = new();
        private List<Asiakas> _asiakkaat = new();
        private List<Mokki> _mokit = new();

        // Listasta valittu varaus. null = lomakkeella tehdään uusi varaus.
        private Varaus? _valittu;

        public VarausPage()
        {
            InitializeComponent();
            AlkuPicker.Date = DateTime.Today;
            LoppuPicker.Date = DateTime.Today.AddDays(1);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // Valikot päivitetään joka kerta, jotta uudet asiakkaat ja mökit näkyvät
                _asiakkaat = AsiakasService.HaeAsiakkaat("");
                _mokit = await _mokkiService.HaeKaikkiMokitAsync();
                AsiakasPicker.ItemsSource = _asiakkaat;
                MokkiPicker.ItemsSource = _mokit;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", DbApu.VirheTeksti(ex), "OK");
                return;
            }

            await LataaAsync();
        }

        private async Task LataaAsync()
        {
            try
            {
                VarausLista.ItemsSource = await _service.HaeVarauksetAsync(HakuEntry.Text);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", DbApu.VirheTeksti(ex), "OK");
            }
        }

        private async void OnHaeClicked(object sender, EventArgs e)
        {
            await LataaAsync();
        }

        private void OnValintaMuuttui(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not Varaus v)
                return;

            _valittu = v;
            LomakeOtsikko.Text = "Muokkaa varausta";
            AsiakasPicker.SelectedItem = _asiakkaat.FirstOrDefault(a => a.AsiakasID == v.AsiakasId);
            MokkiPicker.SelectedItem = _mokit.FirstOrDefault(m => m.MokkiId == v.MokkiId);
            AlkuPicker.Date = v.VarattuAlkupvm.Date;
            LoppuPicker.Date = v.VarattuLoppupvm.Date;
            PoistaButton.IsEnabled = true;
        }

        private void OnUusiClicked(object sender, EventArgs e)
        {
            TyhjennaLomake();
        }

        private void TyhjennaLomake()
        {
            _valittu = null;
            VarausLista.SelectedItem = null;
            LomakeOtsikko.Text = "Uusi varaus";
            AsiakasPicker.SelectedItem = null;
            MokkiPicker.SelectedItem = null;
            AlkuPicker.Date = DateTime.Today;
            LoppuPicker.Date = DateTime.Today.AddDays(1);
            PoistaButton.IsEnabled = false;
        }

        private async void OnTallennaClicked(object sender, EventArgs e)
        {
            var asiakas = AsiakasPicker.SelectedItem as Asiakas;
            var mokki = MokkiPicker.SelectedItem as Mokki;
            DateTime alku = AlkuPicker.Date.Date;
            DateTime loppu = LoppuPicker.Date.Date;

            // Syötteiden tarkistus
            var virheet = new List<string>();
            if (asiakas == null)
                virheet.Add("Valitse asiakas.");
            if (mokki == null)
                virheet.Add("Valitse mökki.");
            if (loppu <= alku)
                virheet.Add("Loppupäivän pitää olla alkupäivän jälkeen.");

            if (virheet.Count > 0 || asiakas == null || mokki == null)
            {
                await DisplayAlert("Tarkista tiedot", string.Join("\n", virheet), "OK");
                return;
            }

            var varaus = new Varaus
            {
                VarausId = _valittu?.VarausId ?? 0,
                AsiakasId = asiakas.AsiakasID,
                MokkiId = mokki.MokkiId,
                VarattuAlkupvm = alku,
                VarattuLoppupvm = loppu
            };

            try
            {
                // Estetään päällekkäiset varaukset samalle mökille
                if (await _service.OnkoPaallekkainenAsync(varaus.MokkiId, alku, loppu, varaus.VarausId))
                {
                    await DisplayAlert("Mökki on varattu",
                        "Mökillä on jo varaus, joka osuu valitulle ajalle. Valitse toinen ajankohta tai mökki.", "OK");
                    return;
                }

                if (_valittu == null)
                    await _service.LisaaVarausAsync(varaus);
                else
                    await _service.MuokkaaVarausAsync(varaus);

                await DisplayAlert("Tallennettu", "Varaus tallennettiin.", "OK");
                TyhjennaLomake();
                await LataaAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", DbApu.VirheTeksti(ex), "OK");
            }
        }

        private async void OnPoistaClicked(object sender, EventArgs e)
        {
            if (_valittu == null)
            {
                await DisplayAlert("Poisto", "Valitse ensin varaus listasta.", "OK");
                return;
            }

            bool ok = await DisplayAlert("Poisto", $"Poistetaanko varaus {_valittu.Otsikko} ({_valittu.Ajankohta})?", "Poista", "Peruuta");
            if (!ok)
                return;

            try
            {
                await _service.PoistaVarausAsync(_valittu.VarausId);
                TyhjennaLomake();
                await LataaAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", DbApu.VirheTeksti(ex), "OK");
            }
        }
    }
}
