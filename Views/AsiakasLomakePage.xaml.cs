using tuotanto1.Models;
using tuotanto1.Services;

namespace tuotanto1.Views
{
    public partial class AsiakasLomakePage : ContentPage
    {
        public AsiakasLomakePage()
        {
            InitializeComponent();
        }

        private async void Tallenna_Clicked(object sender, EventArgs e)
        {
            // Luo uusi asiakas-olio
            var uusiAsiakas = new Asiakas
            {
                Etunimi = EtunimiEntry.Text,
                Sukunimi = SukunimiEntry.Text,
                Lahiosoite = LahiosoiteEntry.Text,
                Postinumero = PostinumeroEntry.Text,
                Sahkoposti = EmailEntry.Text,
                Puhelinnumero = PuhelinnumeroEntry.Text
            };

            // Tallennus MySQL:‰‰n palvelun kautta
            await AsiakasService.LisaaAsiakas(uusiAsiakas);

            await DisplayAlert("Tallennettu", "Asiakas lis‰tty onnistuneesti!", "OK");

            // Palaa takaisin listaan
            await Navigation.PopAsync();
        }
    }
}

