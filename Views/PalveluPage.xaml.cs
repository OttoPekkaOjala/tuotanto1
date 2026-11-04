using tuotanto1.Data;
using tuotanto1.Models;
using tuotanto1.Services;

namespace tuotanto1.Views;

public partial class PalveluPage : ContentPage
{
    private readonly PalveluService _service = new();
    private readonly AlueService _alueService = new();
    private List<Alue> _alueet = new();

    // Listasta valittu palvelu. null = lomakkeella lisätään uusi palvelu.
    private Palvelu? _valittu;

    public PalveluPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            _alueet = await _alueService.HaeAlueetAsync("");
            AluePicker.ItemsSource = _alueet;
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
            PalveluLista.ItemsSource = await _service.HaePalvelutAsync(HakuEntry.Text);
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
        if (e.CurrentSelection.FirstOrDefault() is not Palvelu p)
            return;

        _valittu = p;
        LomakeOtsikko.Text = "Muokkaa palvelua";
        AluePicker.SelectedItem = _alueet.FirstOrDefault(a => a.AlueId == p.AlueId);
        NimiEntry.Text = p.Nimi;
        KuvausEntry.Text = p.Kuvaus;
        HintaEntry.Text = p.Hinta.ToString("0.00");
        AlvEntry.Text = p.Alv.ToString("0.##");
        PoistaButton.IsEnabled = true;
    }

    private void OnUusiClicked(object sender, EventArgs e)
    {
        TyhjennaLomake();
    }

    private void TyhjennaLomake()
    {
        _valittu = null;
        PalveluLista.SelectedItem = null;
        LomakeOtsikko.Text = "Uusi palvelu";
        AluePicker.SelectedItem = null;
        NimiEntry.Text = "";
        KuvausEntry.Text = "";
        HintaEntry.Text = "";
        AlvEntry.Text = "";
        PoistaButton.IsEnabled = false;
    }

    private async void OnTallennaClicked(object sender, EventArgs e)
    {
        var alue = AluePicker.SelectedItem as Alue;
        string nimi = (NimiEntry.Text ?? "").Trim();

        // Syötteiden tarkistus
        var virheet = new List<string>();
        if (alue == null)
            virheet.Add("Valitse alue.");
        if (nimi == "")
            virheet.Add("Palvelun nimi on pakollinen.");
        if (!DbApu.OnkoDesimaali(HintaEntry.Text, out decimal hinta) || hinta < 0 || hinta > 999999)
            virheet.Add("Hinnan pitää olla positiivinen luku, esim. 45,00.");
        if (!DbApu.OnkoDesimaali(AlvEntry.Text, out decimal alv) || alv < 0 || alv > 100)
            virheet.Add("ALV:n pitää olla prosenttiluku väliltä 0–100.");

        if (virheet.Count > 0 || alue == null)
        {
            await DisplayAlert("Tarkista tiedot", string.Join("\n", virheet), "OK");
            return;
        }

        var palvelu = new Palvelu
        {
            AlueId = alue.AlueId,
            Nimi = nimi,
            Kuvaus = (KuvausEntry.Text ?? "").Trim(),
            Hinta = hinta,
            Alv = alv
        };

        try
        {
            if (_valittu == null)
            {
                await _service.LisaaPalveluAsync(palvelu);
            }
            else
            {
                palvelu.PalveluId = _valittu.PalveluId;
                await _service.MuokkaaPalveluAsync(palvelu);
            }

            await DisplayAlert("Tallennettu", "Palvelun tiedot tallennettiin.", "OK");
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
            await DisplayAlert("Poisto", "Valitse ensin palvelu listasta.", "OK");
            return;
        }

        bool ok = await DisplayAlert("Poisto", $"Poistetaanko palvelu {_valittu.Nimi}?", "Poista", "Peruuta");
        if (!ok)
            return;

        try
        {
            await _service.PoistaPalveluAsync(_valittu.PalveluId);
            TyhjennaLomake();
            await LataaAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Virhe", DbApu.VirheTeksti(ex), "OK");
        }
    }
}
