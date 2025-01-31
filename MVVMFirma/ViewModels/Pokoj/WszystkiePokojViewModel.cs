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
    public class WszystkiePokojViewModel : WszystkieViewModel<PokojForAllView>
    {
        #region Constructor
        public WszystkiePokojViewModel()
            : base("Pokoje")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.Pokoj.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<Pokoj> query)
        {
            var result = query.Select(pokoj => new PokojForAllView
            {
                IdPokoju = pokoj.IdPokoju,
                NrPokoju = pokoj.NrPokoju,
                TypPokojuNazwa = pokoj.TypPokoju.Nazwa,
                KlasaPokojuNazwa = pokoj.KlasaPokoju.Nazwa,
                CzyZajety = hotelEntities.Rezerwacja.Any(r =>
                    r.IdPokoju == pokoj.IdPokoju &&
                    r.DataZameldowania <= DateTime.Now &&
                    r.DataWymeldowania >= DateTime.Now)
            }).ToList();

            List = new ObservableCollection<PokojForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show(
                    $"Czy na pewno chcesz usunąć wybrany pokój:\n{SelectedItem.NrPokoju}?",
                    "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.Pokoj.FirstOrDefault(p => p.IdPokoju == SelectedItem.IdPokoju);
                    if (itemToDelete != null)
                    {
                        hotelEntities.Pokoj.Remove(itemToDelete);
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdPokoju);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "PokojRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Sort and Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Numer pokoju", "Typ pokoju", "Klasa pokoju", "Status pokoju" };
        }

        public override void Sort()
        {
            var query = hotelEntities.Pokoj.AsQueryable();

            switch (SortField)
            {
                case "Numer pokoju":
                    query = query.OrderBy(p => p.NrPokoju);
                    break;

                case "Typ pokoju":
                    query = query.OrderBy(p => p.TypPokoju.Nazwa);
                    break;

                case "Klasa pokoju":
                    query = query.OrderBy(p => p.KlasaPokoju.Nazwa);
                    break;

                case "Status pokoju":
                    query = query.OrderByDescending(p => hotelEntities.Rezerwacja.Any(r =>
                        r.IdPokoju == p.IdPokoju &&
                        r.DataZameldowania <= DateTime.Now &&
                        r.DataWymeldowania >= DateTime.Now));
                    break;

                default:
                    break;
            }

            Reload(query);
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Numer pokoju", "Typ pokoju", "Klasa pokoju"};
        }

        public override void Find()
        {
            var query = hotelEntities.Pokoj.AsQueryable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Numer pokoju":
                        query = query.Where(p => p.NrPokoju.Contains(FindTextBox));
                        break;

                    case "Typ pokoju":
                        query = query.Where(p => p.TypPokoju.Nazwa.Contains(FindTextBox));
                        break;

                    case "Klasa pokoju":
                        query = query.Where(p => p.KlasaPokoju.Nazwa.Contains(FindTextBox));
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