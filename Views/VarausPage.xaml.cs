using tuotanto1.ViewModels;

namespace tuotanto1.Views
{
    public partial class VarausPage : ContentPage
    {
        private readonly VarausViewModel _vm = new();

        public VarausPage()
        {
            InitializeComponent();
            BindingContext = _vm;
        }

        private async void Lataa_Clicked(object sender, EventArgs e)
        {
            await _vm.LataaVarauksetAsync();
        }

        private async void Lisaa_Clicked(object sender, EventArgs e)
        {
            await _vm.LisaaVarausAsync(new Models.Varaus
            {
                AsiakasId = 1,
                MokkiId = 1,
                VarattuAlkupvm = DateTime.Now,
                VarattuLoppupvm = DateTime.Now.AddDays(3)
            });

            await _vm.LataaVarauksetAsync();
        }
    }
}
