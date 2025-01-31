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
    public class WszystkieObecnoscViewModel : WszystkieViewModel<ObecnoscForAllView>
    {
        #region Constructor
        public WszystkieObecnoscViewModel()
            : base("Obecności")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.Obecnosc.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<Obecnosc> query)
        {
            var result = query.Select(obecnosc => new ObecnoscForAllView
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
            }).ToList();

            List = new ObservableCollection<ObecnoscForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show(
                    $"Czy na pewno chcesz usunąć obecność dla pracownika:\n{SelectedItem.PracownikImie} {SelectedItem.PracownikNazwisko}?",
                    "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.Obecnosc.FirstOrDefault(o => o.IdObecnosci == SelectedItem.IdObecnosci);
                    if (itemToDelete != null)
                    {
                        hotelEntities.Obecnosc.Remove(itemToDelete);
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

        #region Sort and Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Imie", "Nazwisko", "Data obecności","Obecność","Godzina rozpoczęcia", "Godzina zakończenia", "Usprawiedliwiony" };
        }

        public override void Sort()
        {
            var query = hotelEntities.Obecnosc.AsQueryable();

            switch (SortField)
            {
                case "Imie":
                    query = query.OrderBy(o => o.Pracownik.Imie);
                    break;

                case "Nazwisko":
                    query = query.OrderBy(o => o.Pracownik.Nazwisko);
                    break;

                case "Data obecności":
                    query = query.OrderBy(o => o.Data);
                    break;

                case "Obecność":
                    query = query.OrderBy(o => o.CzyObecny);
                    break;

                case "Godzina rozpoczęcia":
                    query = query.OrderBy(o => o.GodzinaRozpoczecia);
                    break;

                case "Godzina zakończenia":
                    query = query.OrderBy(o => o.GodzinaZakonczenia);
                    break;

                case "Usprawiedliwiony":
                    query = query.OrderBy(o => o.CzyUsprawiedliwiony);
                    break;

                default:
                    break;
            }

            Reload(query);
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Imie", "Nazwisko", "Uwagi" };
        }

        public override void Find()
        {
            var query = hotelEntities.Obecnosc.AsQueryable();

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Imie":
                        query = query.Where(o => o.Pracownik.Imie.Contains(FindTextBox));
                        break;

                    case "Nazwisko":
                        query = query.Where(o => o.Pracownik.Nazwisko.Contains(FindTextBox));
                        break;

                    case "Uwagi":
                        query = query.Where(o => o.Uwagi.Contains(FindTextBox));
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
