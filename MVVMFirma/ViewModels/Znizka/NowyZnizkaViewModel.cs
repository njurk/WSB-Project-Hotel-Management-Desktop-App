using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyZnizkaViewModel : JedenViewModel<Znizka>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Properties
        public string Nazwa
        {
            get
            {
                return item.Nazwa;
            }
            set
            {
                item.Nazwa = value;
                OnPropertyChanged(() => Nazwa);
            }
        }

        public string Wartosc
        {
            get
            {
                return item.Wartosc;
            }
            set
            {
                item.Wartosc = value;
                OnPropertyChanged(() => Wartosc);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdZnizki == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.Znizka.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.Znizka.FirstOrDefault(f => f.IdZnizki == item.IdZnizki);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("ZnizkaRefresh");
        }
        #endregion

        #region Constructor
        public NowyZnizkaViewModel()
            : base("Zniżka")
        {
            db = new HotelEntities();
            item = new Znizka();
        }

        public NowyZnizkaViewModel(int itemId)
            : base("Edycja zniżki")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym przez klasę MainWindowViewModel
            item = db.Znizka.FirstOrDefault(v => v.IdZnizki == itemId);

            if (item != null)
            {
                Nazwa = item.Nazwa;
                Wartosc = item.Wartosc;
            }
        }

        #endregion

        #region Validation
        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Nazwa):
                    return StringValidator.ContainsOnlyLettersWithSpaces(Nazwa) ? "wprowadź poprawną nazwę (nie używaj cyfr ani znaków specjalnych)" : string.Empty;

                case nameof(Wartosc):
                    return !StringValidator.IsNumberInRange(Wartosc, 1, 100) ? "wprowadź liczbę całkowitą, bez znaków specjalnych, z zakresu 1-100" : string.Empty;

                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}
