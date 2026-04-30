using tuotanto1.Views;

namespace tuotanto1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("LisaaAsiakasPage", typeof(LisaaAsiakasPage));

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(AsiakasPage), typeof(AsiakasPage));
            Routing.RegisterRoute(nameof(MokkiPage), typeof(MokkiPage));
            

        }
    }
}
