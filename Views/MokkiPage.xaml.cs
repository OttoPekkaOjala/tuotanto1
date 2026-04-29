using tuotanto1.ViewModels;

namespace tuotanto1.Views
{
    public partial class MokkiPage : ContentPage
    {
        private readonly MokkiViewModel _vm = new();

        public MokkiPage()
        {
            InitializeComponent();
            BindingContext = _vm;
        }

        private async void Lataa_Clicked(object sender, EventArgs e)
        {
            await _vm.LataaMokitAsync();
        }

        private async void Lisaa_Clicked(object sender, EventArgs e)
        {
            await _vm.LisaaMokkiAsync(new Models.Mokki
            {
                AlueId = 1,
                Nimi = "Testimökki",
                Kuvaus = "Testikuvaus",
                Katuosoite = "Mökkitie 1",
                Postinumero = "70100",
                Henkilomaara = 4,
                Hinta = 120
            });

            await _vm.LataaMokitAsync();
        }
    }
}
