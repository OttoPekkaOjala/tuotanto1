using tuotanto1.Data;
using tuotanto1.Models;
using tuotanto1.Services;

namespace tuotanto1.Views;

public partial class AsiakasPage : ContentPage
{
    // Listasta valittu asiakas. null = lomakkeella lisätään uusi asiakas.
    private Asiakas? _valittu;

    public AsiakasPage()
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
            AsiakasLista.ItemsSource = AsiakasService.HaeAsiakkaat(HakuEntry.Text);
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
        if (e.CurrentSelection.FirstOrDefault() is not Asiakas a)
            return;

        _valittu = a;
        LomakeOtsikko.Text = "Muokkaa asiakasta";
        EtunimiEntry.Text = a.Etunimi;
        SukunimiEntry.Text = a.Sukunimi;
        LahiosoiteEntry.Text = a.Lahiosoite;
        PostinumeroEntry.Text = a.Postinumero;
        EmailEntry.Text = a.Sahkoposti;
        PuhelinEntry.Text = a.Puhelinnumero;
        PoistaButton.IsEnabled = true;
    }

    private void OnUusiClicked(object sender, EventArgs e)
    {
        TyhjennaLomake();
    }

    private void TyhjennaLomake()
    {
        _valittu = null;
        AsiakasLista.SelectedItem = null;
        LomakeOtsikko.Text = "Uusi asiakas";
        EtunimiEntry.Text = "";
        SukunimiEntry.Text = "";
        LahiosoiteEntry.Text = "";
        PostinumeroEntry.Text = "";
        EmailEntry.Text = "";
        PuhelinEntry.Text = "";
        PoistaButton.IsEnabled = false;
    }

    private async void OnTallennaClicked(object sender, EventArgs e)
    {
        var asiakas = new Asiakas
        {
            Etunimi = (EtunimiEntry.Text ?? "").Trim(),
            Sukunimi = (SukunimiEntry.Text ?? "").Trim(),
            Lahiosoite = (LahiosoiteEntry.Text ?? "").Trim(),
            Postinumero = (PostinumeroEntry.Text ?? "").Trim(),
            Sahkoposti = (EmailEntry.Text ?? "").Trim(),
            Puhelinnumero = (PuhelinEntry.Text ?? "").Trim()
        };

        // Syötteiden tarkistus
        var virheet = new List<string>();
        if (asiakas.Etunimi == "")
            virheet.Add("Etunimi on pakollinen.");
        if (asiakas.Sukunimi == "")
            virheet.Add("Sukunimi on pakollinen.");
        if (!DbApu.OnkoPostinumero(asiakas.Postinumero))
            virheet.Add("Postinumeron pitää olla 5 numeroa.");
        if (asiakas.Sahkoposti != "" && (!asiakas.Sahkoposti.Contains('@') || !asiakas.Sahkoposti.Contains('.')))
            virheet.Add("Sähköpostiosoite ei ole kelvollinen.");
        if (asiakas.Puhelinnumero != "" && !asiakas.Puhelinnumero.All(c => char.IsDigit(c) || c == '+' || c == ' ' || c == '-'))
            virheet.Add("Puhelinnumerossa saa olla vain numeroita, välilyöntejä, + ja -.");

        if (virheet.Count > 0)
        {
            await DisplayAlert("Tarkista tiedot", string.Join("\n", virheet), "OK");
            return;
        }

        try
        {
            if (_valittu == null)
            {
                await AsiakasService.LisaaAsiakas(asiakas);
            }
            else
            {
                asiakas.AsiakasID = _valittu.AsiakasID;
                await AsiakasService.MuokkaaAsiakas(asiakas);
            }

            await DisplayAlert("Tallennettu", "Asiakkaan tiedot tallennettiin.", "OK");
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
            await DisplayAlert("Poisto", "Valitse ensin asiakas listasta.", "OK");
            return;
        }

        bool ok = await DisplayAlert("Poisto", $"Poistetaanko asiakas {_valittu.Nimi}?", "Poista", "Peruuta");
        if (!ok)
            return;

        try
        {
            AsiakasService.PoistaAsiakas(_valittu.AsiakasID);
            TyhjennaLomake();
            await LataaAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Virhe", DbApu.VirheTeksti(ex), "OK");
        }
    }
}
