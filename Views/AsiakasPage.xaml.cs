namespace tuotanto1.Views;
using tuotanto1.ViewModels;

public partial class AsiakasPage : ContentPage
{
    public AsiakasPage()
    {
        InitializeComponent();                 // Lataa XAML-sisällön
        BindingContext = new AsiakasViewModel(); // Asettaa ViewModelin
    }

    private void OnSearchChanged(object sender, TextChangedEventArgs e)
    {
        var vm = BindingContext as AsiakasViewModel;
        vm.Hakusana = e.NewTextValue;
    }
}

