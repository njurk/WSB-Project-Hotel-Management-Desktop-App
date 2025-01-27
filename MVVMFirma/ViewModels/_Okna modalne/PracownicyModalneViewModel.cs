using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class PracownicyModalneViewModel : ModalViewModel<PracownikForAllView>
    {
        #region DB
        private readonly HotelEntities db;
        #endregion

        #region Constructor
        public PracownicyModalneViewModel()
        {
            db = new HotelEntities();
        }
        #endregion

        #region Load
        public override void Load()
        {
            List = new ObservableCollection<PracownikForAllView>(
                from pracownik in db.Pracownik
                select new PracownikForAllView
                {
                    IdPracownika = pracownik.IdPracownika,
                    StanowiskoNazwa = pracownik.Stanowisko.Nazwa,
                    Imie = pracownik.Imie,
                    Nazwisko = pracownik.Nazwisko,
                    Ulica = pracownik.Ulica,
                    NrDomu = pracownik.NrDomu,
                    NrLokalu = pracownik.NrLokalu,
                    KodPocztowy = pracownik.KodPocztowy,
                    Miasto = pracownik.Miasto,
                    Kraj = pracownik.Kraj.Nazwa,
                    DataUrodzenia = pracownik.DataUrodzenia,
                    Email = pracownik.Email,
                    Telefon = pracownik.Telefon
                }
            );
        }
        #endregion

        #region Send selected item
        public override void SendItem()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(SelectedItem.IdPracownika);
                Cancel();
            }
        }
        #endregion
    }
}
