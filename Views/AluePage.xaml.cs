using tuotanto1.Data;
using tuotanto1.Models;
using tuotanto1.Services;

namespace tuotanto1.Views;

public partial class AluePage : ContentPage
{
    private readonly AlueService _service = new();

    // Listasta valittu alue. null = lomakkeella lisätään uusi alue.
    private Alue? _valittu;

    public AluePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LataaAsync();
    }

    private async Task LataaAsync()
    {
        try
        {
            AlueLista.ItemsSource = await _service.HaeAlueetAsync(HakuEntry.Text);
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
        if (e.CurrentSelection.FirstOrDefault() is not Alue a)
            return;

        _valittu = a;
        LomakeOtsikko.Text = "Muokkaa aluetta";
        NimiEntry.Text = a.Nimi;
        PoistaButton.IsEnabled = true;
    }

    private void OnUusiClicked(object sender, EventArgs e)
    {
        TyhjennaLomake();
    }

    private void TyhjennaLomake()
    {
        _valittu = null;
        AlueLista.SelectedItem = null;
        LomakeOtsikko.Text = "Uusi alue";
        NimiEntry.Text = "";
        PoistaButton.IsEnabled = false;
    }

    private async void OnTallennaClicked(object sender, EventArgs e)
    {
        string nimi = (NimiEntry.Text ?? "").Trim();

        if (nimi == "")
        {
            await DisplayAlert("Tarkista tiedot", "Alueen nimi on pakollinen.", "OK");
            return;
        }

        try
        {
            if (_valittu == null)
                await _service.LisaaAlueAsync(new Alue { Nimi = nimi });
            else
                await _service.MuokkaaAlueAsync(new Alue { AlueId = _valittu.AlueId, Nimi = nimi });

            await DisplayAlert("Tallennettu", "Alue tallennettiin.", "OK");
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
            await DisplayAlert("Poisto", "Valitse ensin alue listasta.", "OK");
            return;
        }

        bool ok = await DisplayAlert("Poisto", $"Poistetaanko alue {_valittu.Nimi}?", "Poista", "Peruuta");
        if (!ok)
            return;

        try
        {
            await _service.PoistaAlueAsync(_valittu.AlueId);
            TyhjennaLomake();
            await LataaAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Virhe", DbApu.VirheTeksti(ex), "OK");
        }
    }
}
