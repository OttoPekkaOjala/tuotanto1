using tuotanto1.ViewModels;

namespace tuotanto1.Views
{
    public partial class LaskuPage : ContentPage
    {
        private readonly LaskuViewModel _vm = new();

        public LaskuPage()
        {
            InitializeComponent();
            BindingContext = _vm;
        }

        private async void Lataa_Clicked(object sender, EventArgs e)
        {
            await _vm.LataaLaskutAsync();
        }
    }
}
