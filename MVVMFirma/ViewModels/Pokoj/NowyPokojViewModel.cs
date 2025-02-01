using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyPokojViewModel : JedenViewModel<Pokoj>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Properties
        public string NrPokoju
        {
            get
            {
                return item.NrPokoju;
            }
            set
            {
                item.NrPokoju = value;
                OnPropertyChanged(() => NrPokoju);
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
        #endregion

        #region Items
        public IEnumerable<KeyAndValue> TypPokojuItems
        {
            get
            {
                return new TypPokojuB(db).GetTypPokojuKeyAndValueItems();
            }
        }

        public IEnumerable<KeyAndValue> KlasaPokojuItems
        {
            get
            {
                return new KlasaPokojuB(db).GetKlasaPokojuKeyAndValueItems();
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdPokoju == 0) // dodawanie rekordu = brak ID = insert
            {
                db.Pokoj.Add(item);
            }
            else // edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.Pokoj.FirstOrDefault(f => f.IdPokoju == item.IdPokoju);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("PokojRefresh");
        }
        #endregion

        #region Constructor
        public NowyPokojViewModel()
            : base("Pokój")
        {
            db = new HotelEntities();
            item = new Pokoj();
        }

        public NowyPokojViewModel(int itemId)
            : base("Edycja pokoju")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.Pokoj.FirstOrDefault(p => p.IdPokoju == itemId);

            if (item != null)
            {
                NrPokoju = item.NrPokoju;
                IdTypuPokoju = item.IdTypuPokoju;
                IdKlasyPokoju = item.IdKlasyPokoju;
            }
        }
        #endregion

        #region Validation
        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(NrPokoju):
                    return !StringValidator.IsPositiveNumber(NrPokoju) ? "wprowadź poprawny numer pokoju" : string.Empty;

                case nameof(IdTypuPokoju):
                    return IdTypuPokoju <= 0 ? "wybierz typ pokoju" : string.Empty;

                case nameof(IdKlasyPokoju):
                    return IdKlasyPokoju <= 0 ? "wybierz klasę pokoju" : string.Empty;

                default:
                    return string.Empty;

            }
        }

        public string ValidateDuplicate()
        {
            var istniejacyRekord = db.Pokoj.FirstOrDefault(p =>
                p.NrPokoju == NrPokoju &&
                p.IdTypuPokoju == IdTypuPokoju &&
                p.IdKlasyPokoju == IdKlasyPokoju &&
                p.IdPokoju != item.IdPokoju);

            if (istniejacyRekord != null)
                return $"istnieje już identyczny pokój. ID: {istniejacyRekord.IdPokoju}. Możesz go edytować lub usunąć i dodać na nowo.";

            return string.Empty;
        }
        #endregion
    }
}
