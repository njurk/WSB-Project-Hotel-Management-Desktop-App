using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkiePokojViewModel : WszystkieViewModel<PokojForAllView>
    {
        #region Constructor
        public WszystkiePokojViewModel()
            : base("Pokoje")
        {
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PokojForAllView>
                (
                    from pokoj in hotelEntities.Pokoj
                    select new PokojForAllView
                    {
                        IdPokoju = pokoj.IdPokoju,
                        NrPokoju = pokoj.NrPokoju,
                        TypPokojuNazwa = pokoj.TypPokoju.Nazwa,
                        KlasaPokojuNazwa = pokoj.KlasaPokoju.Nazwa,
                        StatusPokojuNazwa = pokoj.StatusPokoju.Nazwa,
                        PietroNr = pokoj.Pietro.NrPietra
                    }
                );
        }
        public override void Delete()
        {
            MessageBoxResult delete = MessageBox.Show("Czy na pewno chcesz usunąć wybrany pokój:\n" + SelectedItem.NrPokoju, "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (SelectedItem != null && delete == MessageBoxResult.Yes)
            {
                hotelEntities.Pokoj.Remove(hotelEntities.Pokoj.FirstOrDefault(f => f.IdPokoju == SelectedItem.IdPokoju));
                hotelEntities.SaveChanges();
                List.Remove(SelectedItem);
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdPokoju);
            }
        }

        // OnMessageReceived obsługuje otrzymaną wiadomość, w tym przypadku odświeżenie widoku
        private void OnMessageReceived(string message)
        {
            if (message == "PokojRefresh")
            {
                Load();
            }
        }
        #endregion
    }
}
