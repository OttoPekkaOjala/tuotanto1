namespace tuotanto1.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Asiakkaat_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Asiakkaat");
        }

        private async void Mokit_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Mökit");
        }

        private async void Varaukset_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Varaukset");
        }

        private async void Laskut_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Laskut");
        }

        private async void Raportit_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Raportit");
        }
    }
}
