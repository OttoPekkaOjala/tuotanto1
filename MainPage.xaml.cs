namespace tuotanto1;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await TestDatabase();
    }

    private async void Asiakkaat_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.AsiakasPage());
    }

    private async void Mokit_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.MokkiPage());
    }

    private async void Varaukset_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.VarausPage());
    }

    private async void Laskut_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.LaskuPage());
    }

    private async void Raportit_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.RaportitPage());
    }

    private async Task TestDatabase()
    {
        try
        {
            var db = new DatabaseService();
            bool ok = await db.TestConnectionAsync();

            await DisplayAlert("Tietokanta", ok ? "Yhteys toimii!" : "Yhteys EI toimi", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Virhe", ex.Message, "OK");
        }
    }


}
