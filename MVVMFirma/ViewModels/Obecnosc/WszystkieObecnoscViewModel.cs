using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieObecnoscViewModel : WszystkieViewModel<ObecnoscForAllView>
    {
        #region Constructor
        public WszystkieObecnoscViewModel()
            : base("Obecności")
        {
            // Rejestracja odbierania wiadomości o odświeżeniu listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<ObecnoscForAllView>
                (
                    from obecnosc in hotelEntities.Obecnosc
                    select new ObecnoscForAllView
                    {
                        IdObecnosci = obecnosc.IdObecnosci,
                        PracownikImie = obecnosc.Pracownik.Imie,
                        PracownikNazwisko = obecnosc.Pracownik.Nazwisko,
                        Data = obecnosc.Data,
                        CzyObecny = obecnosc.CzyObecny,
                        GodzinaRozpoczecia = obecnosc.GodzinaRozpoczecia,
                        GodzinaZakonczenia = obecnosc.GodzinaZakonczenia,
                        CzyUsprawiedliwiony = obecnosc.CzyUsprawiedliwiony,
                        Uwagi = obecnosc.Uwagi
                    }
                );
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                MessageBoxResult delete = MessageBox.Show(
                    $"Czy na pewno chcesz usunąć obecność dla pracownika:\n{SelectedItem.PracownikImie} {SelectedItem.PracownikNazwisko}?",
                    "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (delete == MessageBoxResult.Yes)
                {
                    hotelEntities.Obecnosc.Remove(hotelEntities.Obecnosc.FirstOrDefault(o => o.IdObecnosci == SelectedItem.IdObecnosci));
                    hotelEntities.SaveChanges();
                    Load();
                }
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdObecnosci);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "ObecnoscRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
