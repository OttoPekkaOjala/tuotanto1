using System.Collections.ObjectModel;
using tuotanto1.Models;
using tuotanto1.Services;


using System.Collections.ObjectModel;
using System.Windows.Input;
using tuotanto1.Services;
public class AsiakasViewModel : BaseViewModel
{
    public ObservableCollection<Asiakas> Asiakkaat { get; set; } = new();

    private string hakusana;
    public string Hakusana
    {
        get => hakusana;
        set
        {
            hakusana = value;
            OnPropertyChanged();
            HaeAsiakkaat();
        }
    }

    public ICommand LisaaAsiakasCommand { get; }
    public ICommand MuokkaaAsiakastaCommand { get; }
    public ICommand PoistaAsiakasCommand { get; }

    public AsiakasViewModel()
    {
        LisaaAsiakasCommand = new Command(LisaaAsiakas);
        MuokkaaAsiakastaCommand = new Command<Asiakas>(MuokkaaAsiakasta);
        PoistaAsiakasCommand = new Command<Asiakas>(PoistaAsiakas);

        HaeAsiakkaat();
    }

    private void HaeAsiakkaat()
    {
        Asiakkaat.Clear();
        var lista = AsiakasService.HaeAsiakkaat(Hakusana);

        foreach (var a in lista)
            Asiakkaat.Add(a);
    }

    private async void LisaaAsiakas()
    {
        await Shell.Current.GoToAsync("LisaaAsiakasPage");
    }

    private void MuokkaaAsiakasta(Asiakas asiakas)
    {
        // Navigointi muokkauslomakkeelle
    }

    private void PoistaAsiakas(Asiakas asiakas)
    {
        AsiakasService.PoistaAsiakas(asiakas.AsiakasID);
        HaeAsiakkaat();
    }
}
