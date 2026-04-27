using tuotanto1.Data;

namespace tuotanto1.Views
{
    public partial class AsiakasPage : ContentPage
    {
        public AsiakasPage()
        {
            InitializeComponent();
            LataaAsiakkaat();
        }

        private void LataaAsiakkaat()
        {
            AsiakasLista.ItemsSource = Database.HaeAsiakkaat();
        }

        private async void LisaaAsiakas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AsiakasLomakePage());
        }
    }
}
