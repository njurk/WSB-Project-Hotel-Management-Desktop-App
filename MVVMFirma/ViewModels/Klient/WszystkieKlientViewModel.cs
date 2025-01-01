using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class WszystkieKlientViewModel : WszystkieViewModel<KlientForAllView>
    {
        #region Constructor
        public WszystkieKlientViewModel()
            : base()
        {
            base.DisplayName = "Klienci";
        }
        public override void Delete()
        {
            if (SelectedItem != null)
            {
                hotelEntities.Klient.Remove(hotelEntities.Klient.FirstOrDefault(f => f.IdKlienta == SelectedItem.IdKlienta));
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
