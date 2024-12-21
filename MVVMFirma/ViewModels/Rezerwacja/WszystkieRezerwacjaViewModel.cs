using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MVVMFirma.ViewModels
{
    public class WszystkieRezerwacjaViewModel : WszystkieViewModel<UdogodnieniaKlasPokojuForAllView>
    {
        #region Constructor
        public WszystkieRezerwacjaViewModel()
        {
            base.DisplayName = "Rezerwacje";
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<RezerwacjaForAllView>
                (
                    from rezerwacja in hotelEntities.Rezerwacja
                    select new RezerwacjaForAllView
                    {
                        IdRezerwacji = rezerwacja.IdRezerwacji,
                        IdKlienta = rezerwacja.IdKlienta,
                        IdKlientaImie = rezerwacja.Klient.Imie,
                        IdKlientaNazwisko = rezerwacja.Klient.Nazwisko,
                        IdPracownika = rezerwacja.IdPracownika,
                        IloscDoroslych = rezerwacja.IloscDoroslych,
                        IloscDzieci = rezerwacja.IloscDzieci,
                        IloscZwierzat = rezerwacja.IloscZwierzat,
                        DataZameldowania = rezerwacja.DataZameldowania,
                        DataWymeldowania = rezerwacja.DataWymeldowania,
                        IdPlatnosci = rezerwacja.IdPlatnosci,
                        DataRezerwacji = rezerwacja.DataRezerwacji,
                        Uwagi = rezerwacja.Uwagi
                    }
                );
        }

        #endregion
    }
}
