using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class KlienciModalneViewModel : ModalViewModel<KlientForAllView>
    {
        #region DB
        private readonly HotelEntities db;
        #endregion

        #region Constructor
        public KlienciModalneViewModel()
        {
            db = new HotelEntities();
        }
        #endregion

        #region Load
        public override void Load()
        {
            List = new ObservableCollection<KlientForAllView>(
                from klient in db.Klient
                select new KlientForAllView
                {
                    IdKlienta = klient.IdKlienta,
                    Imie = klient.Imie,
                    Nazwisko = klient.Nazwisko,
                    Ulica = klient.Ulica,
                    NrDomu = klient.NrDomu,
                    NrLokalu = klient.NrLokalu,
                    KodPocztowy = klient.KodPocztowy,
                    Miasto = klient.Miasto,
                    Kraj = klient.Kraj.Nazwa,
                    Email = klient.Email,
                    Telefon = klient.Telefon,
                    NIP = klient.NIP
                });
        }
        #endregion

        #region Send selected item
        public override void SendItem()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(SelectedItem.IdKlienta);
                Cancel();
            }
        }
        #endregion
    }
}
