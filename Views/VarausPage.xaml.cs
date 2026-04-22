namespace tuotanto1.Views;

public partial class VarausPage : ContentPage
{
    public VarausPage()
    {
        InitializeComponent();
        BindingContext = new ViewModels.VarausViewModel();
    }

    private async void Etusivu_Clicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new MainPage());

    private async void Asiakkaat_Clicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new AsiakasPage());

    private async void Mokit_Clicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new MokkiPage());

    private async void Varaukset_Clicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new VarausPage());

    private async void Laskut_Clicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new LaskuPage());

    private async void Raportit_Clicked(object sender, EventArgs e)
        => await Navigation.PushAsync(new RaportitPage());
}
