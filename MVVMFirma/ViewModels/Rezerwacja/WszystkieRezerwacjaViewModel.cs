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
    public class WszystkieRezerwacjaViewModel : WszystkieViewModel<RezerwacjaForAllView>
    {
        #region Constructor
        public WszystkieRezerwacjaViewModel()
        {
            base.DisplayName = "Rezerwacje";
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                hotelEntities.Rezerwacja.Remove(hotelEntities.Rezerwacja.FirstOrDefault(f => f.IdRezerwacji == SelectedItem.IdRezerwacji));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<RezerwacjaForAllView>
            (
                from rezerwacja in hotelEntities.Rezerwacja
                let sumaPlatnosci = hotelEntities.Platnosc
                    .Where(p => p.IdRezerwacji == rezerwacja.IdRezerwacji)
                    .Sum(p => (decimal?)p.Kwota) ?? 0.00m
                select new RezerwacjaForAllView
                {
                    IdRezerwacji = rezerwacja.IdRezerwacji,
                    NrRezerwacji = rezerwacja.NrRezerwacji,
                    KlientImie = rezerwacja.Klient.Imie,
                    KlientNazwisko = rezerwacja.Klient.Nazwisko,
                    NrPokoju = rezerwacja.Pokoj.NrPokoju,
                    LiczbaDoroslych = rezerwacja.LiczbaDoroslych,
                    LiczbaDzieci = rezerwacja.LiczbaDzieci,
                    CzyZwierzeta = rezerwacja.CzyZwierzeta,
                    DataRezerwacji = rezerwacja.DataRezerwacji,
                    DataZameldowania = rezerwacja.DataZameldowania,
                    DataWymeldowania = rezerwacja.DataWymeldowania,
                    Kwota = rezerwacja.Kwota,
                    CzyZaplacona = sumaPlatnosci >= rezerwacja.Kwota,
                    Uwagi = rezerwacja.Uwagi
                }
            );
        }

        #endregion
    }
}
