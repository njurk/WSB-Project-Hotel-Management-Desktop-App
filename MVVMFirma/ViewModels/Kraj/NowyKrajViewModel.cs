using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyKrajViewModel : JedenViewModel<Kraj>
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
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdKraju == 0) // dodawanie rekordu = brak ID = insert
            {
                db.Kraj.Add(item);
            }
            else // edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.Kraj.FirstOrDefault(f => f.IdKraju == item.IdKraju);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżene listy po zapisie
            Messenger.Default.Send("KrajRefresh");
        }
        #endregion

        #region Constructor
        public NowyKrajViewModel()
            : base("Kraj")
        {
            db = new HotelEntities();
            item = new Kraj();
        }

        public NowyKrajViewModel(int itemId)
            : base("Edycja kraju")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.Kraj.FirstOrDefault(k => k.IdKraju == itemId);

            if (item != null)
            {
                Nazwa = item.Nazwa;
            }
        }
        #endregion

        #region Validation
        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Nazwa):
                    return StringValidator.ContainsOnlyLettersWithSpaces(Nazwa) ? "wprowadź poprawną nazwę kraju (nie używaj cyfr ani znaków specjalnych)" : string.Empty;

                default:
                    return string.Empty;
            }
        }

        public string ValidateDuplicate()
        {
            var istniejacyRekord = db.Kraj.FirstOrDefault(k =>
                k.Nazwa == Nazwa &&
                k.IdKraju != item.IdKraju);

            if (istniejacyRekord != null)
                return $"ten kraj już istnieje. Ma ID {istniejacyRekord.IdKraju}. Możesz go edytować lub usunąć i dodać na nowo.";

            return string.Empty;
        }

        #endregion
    }
}
