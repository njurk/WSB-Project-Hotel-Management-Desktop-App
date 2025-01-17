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
    public class KlienciModalneViewModel : ModalViewModel<KlientForAllView>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public KlienciModalneViewModel()
        {
            db = new HotelEntities();
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<KlientForAllView>
                (
                    from klient in hotelEntities.Klient
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
                    }
                );
        }
        #endregion
    }
}
