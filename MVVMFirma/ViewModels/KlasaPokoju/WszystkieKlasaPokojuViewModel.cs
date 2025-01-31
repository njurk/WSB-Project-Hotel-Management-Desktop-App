using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieKlasaPokojuViewModel : WszystkieViewModel<KlasaPokojuForAllView>
    {
        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.KlasaPokoju.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<KlasaPokoju> query)
        {
            var result = query.Select(klasapokoju => new KlasaPokojuForAllView
            {
                IdKlasyPokoju = klasapokoju.IdKlasyPokoju,
                Nazwa = klasapokoju.Nazwa
            }).ToList();

            List = new ObservableCollection<KlasaPokojuForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show($"Czy na pewno chcesz usunąć wybraną klasę pokoju: {SelectedItem.Nazwa}?",
                    "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.KlasaPokoju.FirstOrDefault(f => f.IdKlasyPokoju == SelectedItem.IdKlasyPokoju);
                    if (itemToDelete != null)
                    {
                        hotelEntities.KlasaPokoju.Remove(itemToDelete);
                        hotelEntities.SaveChanges();
                        Load();
                    }
                }
            }
        }

        public override void Edit()
        {
            if (SelectedItem != null)
            {
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdKlasyPokoju);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "KlasaPokojuRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Constructor
        public WszystkieKlasaPokojuViewModel()
            : base("Klasy pokojów")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Sort and Find
        // tu decydujemy po czym sortować
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa" };
        }

        // tu decydujemy jak sortować
        public override void Sort()
        {
            var query = hotelEntities.KlasaPokoju.AsQueryable();

            switch (SortField)
            {
                case "Nazwa":
                    query = query.OrderBy(k => k.Nazwa);
                    break;

                default:
                    break;
            }

            Reload(query);
        }

        // tu decydujemy po czym wyszukiwać
        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa" };
        }

        // tu decydujemy jak wyszukiwać
        public override void Find()
        {
            var query = hotelEntities.KlasaPokoju.AsQueryable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Nazwa":
                        query = query.Where(k => k.Nazwa.Contains(FindTextBox));
                        break;

                    default:
                        break;
                }
            }

            Reload(query);
        }
        #endregion
    }
}
