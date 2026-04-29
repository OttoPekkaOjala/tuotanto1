using System.Windows.Input;
using tuotanto1.Services;
using tuotanto1.Models;

namespace tuotanto1.ViewModels;

public class LisaaAsiakasViewModel : BaseViewModel
{
    public string Etunimi { get; set; }
    public string Sukunimi { get; set; }
    public string Sahkoposti { get; set; }
    public string Puhelinnumero { get; set; }
    public string Postinumero { get; set; }


    public ICommand TallennaAsiakasCommand { get; }

    public LisaaAsiakasViewModel()
    {
        TallennaAsiakasCommand = new Command(async () => await TallennaAsiakas());
    }

    private async Task TallennaAsiakas()
    {
        Console.WriteLine($"DEBUG: {Etunimi}, {Sukunimi}, {Sahkoposti}, {Puhelinnumero}, {Postinumero}");

        var uusi = new Asiakas
        {
            Etunimi = this.Etunimi,
            Sukunimi = this.Sukunimi,
            Sahkoposti = this.Sahkoposti,
            Puhelinnumero = this.Puhelinnumero,
            Postinumero = this.Postinumero

        };

        await AsiakasService.LisaaAsiakas(uusi);

        await Shell.Current.GoToAsync(".."); // palaa takaisin listaan
    }
}
