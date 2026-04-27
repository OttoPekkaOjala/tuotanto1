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
            await _viewModel.LataaRaportitAsync();
        }
    }
}
