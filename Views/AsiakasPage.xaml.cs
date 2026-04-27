using tuotanto1.ViewModels;

namespace tuotanto1.Views
{
    public partial class AsiakasPage : ContentPage
    {
        private readonly AsiakasViewModel _vm = new();

        public AsiakasPage()
        {
            InitializeComponent();
            BindingContext = _vm;
        }

        private async void Lataa_Clicked(object sender, EventArgs e)
        {
            await _vm.LataaAsiakkaatAsync();
        }

        private async void Lisaa_Clicked(object sender, EventArgs e)
        {
            // Esimerkki: lis‰‰ testiasiakas
            await _vm.LisaaAsiakasAsync(new Models.Asiakas
            {
                Etunimi = "Testi",
                Sukunimi = "Asiakas",
                Lahiosoite = "Katu 1",
                Postinumero = "70100",
                Puhelinnumero = "0401234567",
                Email = "testi@example.com"
            });

            await _vm.LataaAsiakkaatAsync();
        }
    }
}
