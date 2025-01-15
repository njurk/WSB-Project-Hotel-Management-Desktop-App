using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyVATViewModel : JedenViewModel<VAT>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Properties
        public string Stawka
        {
            get
            {
                return item.Stawka;
            }
            set
            {
                item.Stawka = value;
                OnPropertyChanged(() => Stawka);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdVat == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.VAT.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.VAT.FirstOrDefault(f => f.IdVat == item.IdVat);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("VATRefresh");
        }
        #endregion

        #region Constructor
        public NowyVATViewModel()
            : base("Stawka VAT")
        {
            db = new HotelEntities();
            item = new VAT();
        }

        public NowyVATViewModel(int itemId)
            : base("Edycja stawki VAT")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.VAT.FirstOrDefault(v => v.IdVat == itemId);

            if (item != null)
            {
                Stawka = item.Stawka;
            }
        }

        #endregion

        #region Validation
        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Stawka):
                    return !StringValidator.IsNumberInRange(Stawka, 0, 100) ? "wprowadź liczbę całkowitą, bez znaków specjalnych, z zakresu 0-100" : string.Empty;

                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}
