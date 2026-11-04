using tuotanto1.Data;
using tuotanto1.ViewModels;

namespace tuotanto1.Views
{
    public partial class RaportitPage : ContentPage
    {
        private readonly RaportointiViewModel _viewModel;

        public RaportitPage()
        {
            InitializeComponent();
            _viewModel = new RaportointiViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await _viewModel.LataaRaportitAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Virhe", DbApu.VirheTeksti(ex), "OK");
            }
        }
    }
}
