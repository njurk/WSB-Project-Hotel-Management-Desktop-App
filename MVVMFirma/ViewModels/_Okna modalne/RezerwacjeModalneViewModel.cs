using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class RezerwacjeModalneViewModel : ModalViewModel<RezerwacjaForAllView>
    {
        #region DB
        private readonly HotelEntities db;
        private bool _bezFaktury;
        #endregion

        #region Constructor
        public RezerwacjeModalneViewModel()
        {
            db = new HotelEntities();
        }

        public RezerwacjeModalneViewModel(bool bezFaktury)
        {
            db = new HotelEntities();
            _bezFaktury = bezFaktury;
        }
        #endregion

        #region Load
        public override void Load()
        {
            if (_bezFaktury == true)
            {
                List = new ObservableCollection<RezerwacjaForAllView>(
                    from rezerwacja in db.Rezerwacja
                    let sumaPlatnosci = db.Platnosc
                        .Where(p => p.IdRezerwacji == rezerwacja.IdRezerwacji)
                        .Sum(p => (decimal?)p.Kwota) ?? -1m
                    where !db.Faktura.Any(f => f.IdRezerwacji == rezerwacja.IdRezerwacji)
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
            else
            {
                List = new ObservableCollection<RezerwacjaForAllView>(
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
        }
        #endregion

        #region Send selected item
        public override void SendItem()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(SelectedItem.IdRezerwacji);
                Cancel();
            }
        }
        #endregion
    }
}
