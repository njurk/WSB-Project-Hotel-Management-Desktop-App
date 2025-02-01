using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System.Linq;
using System.Windows.Controls;

namespace MVVMFirma.ViewModels
{
    public class NowyKlasaPokojuViewModel : JedenViewModel<KlasaPokoju>
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
            if (item.IdKlasyPokoju == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.KlasaPokoju.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.KlasaPokoju.FirstOrDefault(f => f.IdKlasyPokoju == item.IdKlasyPokoju);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }
            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("KlasaPokojuRefresh");
        }
        #endregion

        #region Constructor
        public NowyKlasaPokojuViewModel()
            : base("Klasa pokoju")
        {
            db = new HotelEntities();
            item = new KlasaPokoju();
        }

        public NowyKlasaPokojuViewModel(int itemId)
            : base("Edycja klasy pokoju")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.KlasaPokoju.FirstOrDefault(k => k.IdKlasyPokoju == itemId);

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

        public string ValidateDuplicate()
        {
            var istniejacyRekord = db.KlasaPokoju.FirstOrDefault(k =>
                k.Nazwa == Nazwa &&
                k.IdKlasyPokoju != item.IdKlasyPokoju);

            if (istniejacyRekord != null)
                return $"istnieje już identyczna klasa pokoju. Ma ID {istniejacyRekord.IdKlasyPokoju}. Możesz ją edytować lub usunąć i dodać na nowo.";

            return string.Empty;
        }

        #endregion
    }
}
