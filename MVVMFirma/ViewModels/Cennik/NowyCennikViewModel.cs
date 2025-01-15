using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class NowyCennikViewModel : JedenViewModel<Cennik>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Properties
        public int IdKlasyPokoju
        {
            get
            {
                return item.IdKlasyPokoju;
            }
            set
            {
                item.IdKlasyPokoju = value;
                OnPropertyChanged(() => IdKlasyPokoju);
            }
        }
        public int IdTypuPokoju
        {
            get
            {
                return item.IdTypuPokoju;
            }
            set
            {
                item.IdTypuPokoju = value;
                OnPropertyChanged(() => IdTypuPokoju);
            }
        }

        public decimal CenaDorosly
        {
            get
            {
                return item.CenaDorosly;
            }
            set
            {
                item.CenaDorosly = value;
                OnPropertyChanged(() => CenaDorosly);
            }
        }

        public decimal CenaDziecko
        {
            get
            {
                return item.CenaDziecko;
            }
            set
            {
                item.CenaDziecko = value;
                OnPropertyChanged(() => CenaDziecko);
            }
        }

        public decimal CenaZwierzeta
        {
            get
            {
                return item.CenaZwierzeta;
            }
            set
            {
                item.CenaZwierzeta = value;
                OnPropertyChanged(() => CenaZwierzeta);
            }
        }
        #endregion

        #region Items
        public IEnumerable<KeyAndValue> KlasaPokojuItems
        {
            get
            {
                return new KlasaPokojuB(db).GetKlasaPokojuKeyAndValueItems();
            }
        }

        public IEnumerable<KeyAndValue> TypPokojuItems
        {
            get
            {
                return new TypPokojuB(db).GetTypPokojuKeyAndValueItems();
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            // Walidacja istnienia cennika
            string validationError = ValidateCennik();
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show(validationError, "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Przerwij zapisywanie, jeśli jest błąd
            }

            if (item.IdCennika == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.Cennik.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.Cennik.FirstOrDefault(f => f.IdCennika == item.IdCennika);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // Wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("CennikRefresh");
        }
        #endregion

        #region Methods
        // metoda sprawdzająca czy istnieje już cennnik dla wybranej pary kluczy obcych
        public string ValidateCennik()
        {
            var istniejacyCennik = db.Cennik.FirstOrDefault(c =>
                c.IdTypuPokoju == IdTypuPokoju &&
                c.IdKlasyPokoju == IdKlasyPokoju &&
                c.IdCennika != item.IdCennika); // Nie porównuj z aktualnym ID

            if (istniejacyCennik != null)
                return $"istnieje cennik dla podanej pary typu i klasy pokoju, ma ID {istniejacyCennik.IdCennika}. Możesz go edytować, lub usunąć i dodać na nowo.";

            return string.Empty;
        }
        #endregion

        #region Constructor
        public NowyCennikViewModel()
            : base("Cennik")
        {
            db = new HotelEntities();
            item = new Cennik();
        }

        public NowyCennikViewModel(int itemId)
            : base("Edycja cennika")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.Cennik.FirstOrDefault(p => p.IdCennika == itemId);

            if (item != null)
            {
                IdKlasyPokoju = item.IdKlasyPokoju;
                IdTypuPokoju = item.IdTypuPokoju;
                CenaDorosly = item.CenaDorosly;
                CenaDziecko = item.CenaDziecko;
                CenaZwierzeta = item.CenaZwierzeta;
            }
        }
        #endregion

        #region Validation
        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IdKlasyPokoju):
                    return IdKlasyPokoju <= 0 ? "wybierz klasę pokoju" : string.Empty;

                case nameof(IdTypuPokoju):
                    return IdTypuPokoju <= 0 ? "wybierz typ pokoju" : string.Empty;

                case nameof(CenaDorosly):
                    return CenaDorosly <= 0 ? "wprowadź poprawną kwotę (większą niż 0)" : string.Empty;

                case nameof(CenaDziecko):
                    return CenaDziecko <= 0 ? "wprowadź poprawną kwotę (większą niż 0)" : string.Empty;

                case nameof(CenaZwierzeta):
                    return CenaZwierzeta <= 0 ? "wprowadź poprawną kwotę (większą niż 0)" : string.Empty;

                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}