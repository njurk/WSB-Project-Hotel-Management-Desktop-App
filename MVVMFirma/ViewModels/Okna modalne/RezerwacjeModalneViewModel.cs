using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MVVMFirma.Models.EntitiesForView;
using GalaSoft.MvvmLight.Messaging;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Runtime.CompilerServices;

namespace MVVMFirma.ViewModels
{
    public class RezerwacjeModalneViewModel : ModalViewModel<RezerwacjaForAllView>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public RezerwacjeModalneViewModel()
        {
            db = new HotelEntities();
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<RezerwacjaForAllView>
            (
                from rezerwacja in db.Rezerwacja
                let sumaPlatnosci = db.Platnosc
                    .Where(p => p.IdRezerwacji == rezerwacja.IdRezerwacji)
                    .Sum(p => (decimal?)p.Kwota) ?? -1m
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
                    Uwagi = rezerwacja.Uwagi,
                    Znizka = rezerwacja.Znizka.Wartosc
                }
            );
        }
        #endregion
    }
}
