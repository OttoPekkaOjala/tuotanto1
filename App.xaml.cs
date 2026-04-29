namespace tuotanto1;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Käynnistä sovellus Shellin kautta
        MainPage = new AppShell();
    }
}
