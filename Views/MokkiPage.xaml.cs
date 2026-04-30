using System.Collections.ObjectModel;

namespace tuotanto1.Views;

public partial class MokkiPage : ContentPage
{
    public ObservableCollection<Mokki> Mokit { get; set; }

    private Mokki _muokattavaMokki;

    public MokkiPage()
    {
        InitializeComponent();

        Mokit = new ObservableCollection<Mokki>
        {
            new Mokki { Nimi = "Rantamökki", Sijainti = "Kuopio" },
            new Mokki { Nimi = "Tunturimökki", Sijainti = "Levi" }
        };

        MokitCollection.ItemsSource = Mokit;
    }

    private void OnAddMokkiClicked(object sender, EventArgs e)
    {
        _muokattavaMokki = null;

        NimiEntry.Text = "";
        SijaintiEntry.Text = "";

        LisaaFrame.IsVisible = true;
    }

    private void OnTallennaClicked(object sender, EventArgs e)
    {
        string nimi = NimiEntry.Text;
        string sijainti = SijaintiEntry.Text;

        if (string.IsNullOrWhiteSpace(nimi) || string.IsNullOrWhiteSpace(sijainti))
        {
            DisplayAlert("Virhe", "Täytä kaikki kentät", "OK");
            return;
        }

        if (_muokattavaMokki == null)
        {
            Mokit.Add(new Mokki
            {
                Nimi = nimi,
                Sijainti = sijainti
            });
        }
        else
        {
            _muokattavaMokki.Nimi = nimi;
            _muokattavaMokki.Sijainti = sijainti;

            MokitCollection.ItemsSource = null;
            MokitCollection.ItemsSource = Mokit;
        }

        LisaaFrame.IsVisible = false;
    }

    private void OnPeruutaClicked(object sender, EventArgs e)
    {
        LisaaFrame.IsVisible = false;
    }

    private void OnMuokkaaClicked(object sender, EventArgs e)
    {
        var mokki = (sender as Button)?.CommandParameter as Mokki;

        if (mokki == null)
            return;

        _muokattavaMokki = mokki;

        NimiEntry.Text = mokki.Nimi;
        SijaintiEntry.Text = mokki.Sijainti;

        LisaaFrame.IsVisible = true;
    }
}

public class Mokki
{
    public string Nimi { get; set; }
    public string Sijainti { get; set; }
}
