using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePracownikViewModel : WszystkieViewModel<PracownikForAllView>
    {
        #region Constructor
        public WszystkiePracownikViewModel()
        {
            base.DisplayName = "Pracownicy";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PracownikForAllView>
                (
                    from pracownik in hotelEntities.Pracownik
                    select new PracownikForAllView
                    {
                        IdPracownika = pracownik.IdPracownika,
                        IdRodzajuPracownikaNazwa = pracownik.RodzajPracownika.Nazwa,
                        Imie = pracownik.Imie,
                        Nazwisko = pracownik.Nazwisko,
                        Ulica = pracownik.Ulica,
                        NrDomu = pracownik.NrDomu,
                        NrLokalu = pracownik.NrLokalu,
                        KodPocztowy = pracownik.KodPocztowy,
                        Miasto = pracownik.Miasto,
                        Kraj = pracownik.Kraj,
                        DataUrodzenia = pracownik.DataUrodzenia,
                        Email = pracownik.Email,
                        Telefon = pracownik.Telefon
                    }
                );
        }
        #endregion

    }
}
