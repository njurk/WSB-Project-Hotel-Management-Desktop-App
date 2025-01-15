using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyTypPokojuViewModel : JedenViewModel<TypPokoju>
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

        public string MaxLiczbaOsob
        {
            get
            {
                return item.MaxLiczbaOsob;
            }
            set
            {
                item.MaxLiczbaOsob = value;
                OnPropertyChanged(() => MaxLiczbaOsob);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdTypuPokoju == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.TypPokoju.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.TypPokoju.FirstOrDefault(f => f.IdTypuPokoju == item.IdTypuPokoju);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("TypPokojuRefresh");
        }
        #endregion

        #region Constructor
        public NowyTypPokojuViewModel()
            : base("Typ pokoju")
        {
            db = new HotelEntities();
            item = new TypPokoju();
        }

        public NowyTypPokojuViewModel(int itemId)
            : base("Edycja typu pokoju")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.TypPokoju.FirstOrDefault(s => s.IdTypuPokoju == itemId);

            if (item != null)
            {
                Nazwa = item.Nazwa;
                MaxLiczbaOsob = item.MaxLiczbaOsob;
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

                case nameof(MaxLiczbaOsob):
                    return !StringValidator.IsPositiveNumber(MaxLiczbaOsob) ? "wprowadź poprawną liczbę całkowitą" : string.Empty;

                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}
