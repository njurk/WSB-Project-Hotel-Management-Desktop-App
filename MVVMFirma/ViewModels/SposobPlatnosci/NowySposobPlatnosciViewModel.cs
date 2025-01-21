using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowySposobPlatnosciViewModel : JedenViewModel<SposobPlatnosci>
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
            if (item.IdSposobuPlatnosci == 0) // dodawanie rekordu = brak ID = insert
            {
                db.SposobPlatnosci.Add(item);
            }
            else // edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.SposobPlatnosci.FirstOrDefault(f => f.IdSposobuPlatnosci == item.IdSposobuPlatnosci);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("SposobPlatnosciRefresh");
        }
        #endregion

        #region Constructor
        public NowySposobPlatnosciViewModel()
            : base("Sposób płatności")
        {
            db = new HotelEntities();
            item = new SposobPlatnosci();
        }

        public NowySposobPlatnosciViewModel(int itemId)
            : base("Edycja sposobu płatności")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.SposobPlatnosci.FirstOrDefault(s => s.IdSposobuPlatnosci == itemId);

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
                    return StringValidator.ContainsOnlyLettersWithSpaces(Nazwa) ? "wprowadź poprawną nazwę (nie używaj cyfr ani znaków specjalnych)" : string.Empty;

                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}
