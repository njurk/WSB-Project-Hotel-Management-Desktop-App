using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class WszystkieTypPokojuViewModel : WszystkieViewModel<TypPokojuForAllView>
    {
        #region Constructor
        public WszystkieTypPokojuViewModel()
            : base("Typy pokojów")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.TypPokoju.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<TypPokoju> query)
        {
            var result = query.Select(typpokoju => new TypPokojuForAllView
            {
                IdTypuPokoju = typpokoju.IdTypuPokoju,
                Nazwa = typpokoju.Nazwa,
                MaxLiczbaOsob = typpokoju.MaxLiczbaOsob
            }).ToList();

            List = new ObservableCollection<TypPokojuForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show($"Czy na pewno chcesz usunąć wybrany typ pokoju:\n{SelectedItem.Nazwa}?"
                    , "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.TypPokoju.FirstOrDefault(t => t.IdTypuPokoju == SelectedItem.IdTypuPokoju);
                    if (itemToDelete != null)
                    {
                        hotelEntities.TypPokoju.Remove(itemToDelete);
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdTypuPokoju);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "TypPokojuRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Sort and Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa", "Maks. liczba gości" };
        }

        public override void Sort()
        {
            var query = hotelEntities.TypPokoju.AsQueryable();

            switch (SortField)
            {
                case "Nazwa":
                    query = query.OrderBy(t => t.Nazwa);
                    break;

                case "Maks. liczba gości":
                    query = query.OrderBy(t => t.MaxLiczbaOsob);
                    break;

                default:
                    break;
            }

            Reload(query);
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa"};
        }

        public override void Find()
        {
            var query = hotelEntities.TypPokoju.AsQueryable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Nazwa":
                        query = query.Where(t => t.Nazwa.Contains(FindTextBox));
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