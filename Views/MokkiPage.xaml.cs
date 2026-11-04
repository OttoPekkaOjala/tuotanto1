using tuotanto1.Data;
using tuotanto1.Models;
using tuotanto1.Services;

namespace tuotanto1.Views;

public partial class MokkiPage : ContentPage
{
    private readonly MokkiService _service = new();
    private readonly AlueService _alueService = new();
    private List<Alue> _alueet = new();

    // Listasta valittu mökki. null = lomakkeella lisätään uusi mökki.
    private Mokki? _valittu;

    public MokkiPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Alueet haetaan joka kerta, jotta uudet alueet näkyvät valikossa
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
            MokkiLista.ItemsSource = await _service.HaeMokitAsync(HakuEntry.Text);
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
        if (e.CurrentSelection.FirstOrDefault() is not Mokki m)
            return;

        _valittu = m;
        LomakeOtsikko.Text = "Muokkaa mökkiä";
        AluePicker.SelectedItem = _alueet.FirstOrDefault(a => a.AlueId == m.AlueId);
        NimiEntry.Text = m.Nimi;
        KatuosoiteEntry.Text = m.Katuosoite;
        PostinumeroEntry.Text = m.Postinumero;
        HintaEntry.Text = m.Hinta.ToString("0.00");
        HenkilomaaraEntry.Text = m.Henkilomaara.ToString();
        KuvausEntry.Text = m.Kuvaus;
        VarusteluEntry.Text = m.Varustelu;
        PoistaButton.IsEnabled = true;
    }

    private void OnUusiClicked(object sender, EventArgs e)
    {
        TyhjennaLomake();
    }

    private void TyhjennaLomake()
    {
        _valittu = null;
        MokkiLista.SelectedItem = null;
        LomakeOtsikko.Text = "Uusi mökki";
        AluePicker.SelectedItem = null;
        NimiEntry.Text = "";
        KatuosoiteEntry.Text = "";
        PostinumeroEntry.Text = "";
        HintaEntry.Text = "";
        HenkilomaaraEntry.Text = "";
        KuvausEntry.Text = "";
        VarusteluEntry.Text = "";
        PoistaButton.IsEnabled = false;
    }

    private async void OnTallennaClicked(object sender, EventArgs e)
    {
        var alue = AluePicker.SelectedItem as Alue;
        string nimi = (NimiEntry.Text ?? "").Trim();
        string postinro = (PostinumeroEntry.Text ?? "").Trim();

        // Syötteiden tarkistus
        var virheet = new List<string>();
        if (alue == null)
            virheet.Add("Valitse alue.");
        if (nimi == "")
            virheet.Add("Mökin nimi on pakollinen.");
        if (!DbApu.OnkoPostinumero(postinro))
            virheet.Add("Postinumeron pitää olla 5 numeroa.");
        if (!DbApu.OnkoDesimaali(HintaEntry.Text, out decimal hinta) || hinta < 0 || hinta > 999999)
            virheet.Add("Hinnan pitää olla positiivinen luku, esim. 120,50.");
        if (!int.TryParse((HenkilomaaraEntry.Text ?? "").Trim(), out int henkilomaara) || henkilomaara < 1 || henkilomaara > 100)
            virheet.Add("Henkilömäärän pitää olla kokonaisluku väliltä 1–100.");

        if (virheet.Count > 0 || alue == null)
        {
            await DisplayAlert("Tarkista tiedot", string.Join("\n", virheet), "OK");
            return;
        }

        var mokki = new Mokki
        {
            AlueId = alue.AlueId,
            Nimi = nimi,
            Katuosoite = (KatuosoiteEntry.Text ?? "").Trim(),
            Postinumero = postinro,
            Hinta = hinta,
            Henkilomaara = henkilomaara,
            Kuvaus = (KuvausEntry.Text ?? "").Trim(),
            Varustelu = (VarusteluEntry.Text ?? "").Trim()
        };

        try
        {
            if (_valittu == null)
            {
                await _service.LisaaMokkiAsync(mokki);
            }
            else
            {
                mokki.MokkiId = _valittu.MokkiId;
                await _service.MuokkaaMokkiAsync(mokki);
            }

            await DisplayAlert("Tallennettu", "Mökin tiedot tallennettiin.", "OK");
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
            await DisplayAlert("Poisto", "Valitse ensin mökki listasta.", "OK");
            return;
        }

        bool ok = await DisplayAlert("Poisto", $"Poistetaanko mökki {_valittu.Nimi}?", "Poista", "Peruuta");
        if (!ok)
            return;

        try
        {
            await _service.PoistaMokkiAsync(_valittu.MokkiId);
            TyhjennaLomake();
            await LataaAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Virhe", DbApu.VirheTeksti(ex), "OK");
        }
    }
}
