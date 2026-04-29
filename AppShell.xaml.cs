using tuotanto1.Views;

namespace tuotanto1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("LisaaAsiakasPage", typeof(LisaaAsiakasPage));

        }
    }
}
