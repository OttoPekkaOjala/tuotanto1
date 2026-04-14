using System.Collections.ObjectModel;
using tuotanto1.Models;

namespace tuotanto1.ViewModels;

public class AsiakasViewModels
{
    public ObservableCollection<Asiakas> Asiakkaat { get; set; }

    public AsiakasViewModels()
    {
        Asiakkaat = new ObservableCollection<Asiakas>
        {
            new Asiakas { AsiakasId = 1, Etunimi = "Jarmo", Sukunimi = "Jokinen", Email = "jamppa@example.com", Puhelinnro = "0401234567" },
            new Asiakas { AsiakasId = 2, Etunimi = "Liisa", Sukunimi = "Lahtinen", Email = "liisa@example.com", Puhelinnro = "0509876543" }
        };
    }
}