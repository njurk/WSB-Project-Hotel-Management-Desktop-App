using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieCennikViewModel : WszystkieViewModel<CennikForAllView>
    {
        #region Constructor
        public WszystkieCennikViewModel()
            : base("Cenniki")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<CennikForAllView>
                (
                    from cennik in hotelEntities.Cennik
                    select new CennikForAllView
                    {
                        IdCennika = cennik.IdCennika,
                        KlasaPokojuNazwa = cennik.KlasaPokoju.Nazwa,
                        TypPokojuNazwa = cennik.TypPokoju.Nazwa,
                        CenaDorosly = cennik.CenaDorosly,
                        CenaDziecko = cennik.CenaDziecko,
                        CenaZwierzeta = cennik.CenaZwierzeta
                    }
                );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrany cennik?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Cennik.Remove(hotelEntities.Cennik.FirstOrDefault(f => f.IdCennika == SelectedItem.IdCennika));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdCennika);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "CennikRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
