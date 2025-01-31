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
    public class WszystkieVATViewModel : WszystkieViewModel<VATForAllView>
    {
        #region Constructor
        public WszystkieVATViewModel()
            : base("Stawki VAT")
        {
            // odbieranie wiadomości odświeżenia listy
            Messenger.Default.Register<string>(this, OnMessageReceived);
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            var query = hotelEntities.VAT.AsQueryable();
            Reload(query);
        }

        private void Reload(IQueryable<VAT> query)
        {
            var result = query.Select(vat => new VATForAllView
            {
                IdVat = vat.IdVat,
                Stawka = vat.Stawka
            }).ToList();

            List = new ObservableCollection<VATForAllView>(result);
        }

        public override void Delete()
        {
            if (SelectedItem != null)
            {
                var delete = MessageBox.Show($"Czy na pewno chcesz usunąć wybraną stawkę VAT:\n{SelectedItem.Stawka}?",
                    "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (delete == MessageBoxResult.Yes)
                {
                    var itemToDelete = hotelEntities.VAT.FirstOrDefault(v => v.IdVat == SelectedItem.IdVat);
                    if (itemToDelete != null)
                    {
                        hotelEntities.VAT.Remove(itemToDelete);
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
                Messenger.Default.Send(DisplayName + "Edit-" + SelectedItem.IdVat);
                SelectedItem = null;
            }
        }

        private void OnMessageReceived(string message)
        {
            if (message == "VATRefresh")
            {
                Load();
            }
        }
        #endregion

        #region Sort and Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Stawka" };
        }

        public override void Sort()
        {
            var query = hotelEntities.VAT.AsQueryable();

            var result = query.Select(v => new VATForAllView
            {
                IdVat = v.IdVat,
                Stawka = v.Stawka
            }).AsEnumerable(); // sortowanie lokalne z powodu konieczności konwersji stawki (query nie rozpozna funkcji TryParse)

            switch (SortField)
            {
                case "Stawka":
                    result = result.OrderBy(v => int.TryParse(v.Stawka.ToString(), out var stawkaInt) ? stawkaInt : 0);
                    break;

                default:
                    break;
            }

            List = new ObservableCollection<VATForAllView>(result);
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Stawka" };
        }

        public override void Find()
        {
            var query = hotelEntities.VAT.AsQueryable();

            var result = query.Select(v => new VATForAllView
            {
                IdVat = v.IdVat,
                Stawka = v.Stawka
            }).AsEnumerable(); // wyszukiwanie lokalne, jak powyżej

            if (!string.IsNullOrEmpty(FindTextBox))
            {
                switch (FindField)
                {
                    case "Stawka":
                        if (int.TryParse(FindTextBox, out var findStawkaInt))
                        {
                            result = result.Where(v => int.TryParse(v.Stawka.ToString(), out var stawkaInt) && stawkaInt == findStawkaInt);
                        }
                        break;

                    default:
                        break;
                }
            }

            List = new ObservableCollection<VATForAllView>(result);
        }
        #endregion
    }
}
