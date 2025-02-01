using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Navigation;

namespace MVVMFirma.ViewModels
{
    public class NowyKlientViewModel : JedenViewModel<Klient>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Properties
        public string Imie
        {
            get
            {
                return item.Imie;
            }
            set
            {
                item.Imie = value;
                OnPropertyChanged(() => Imie);
            }
        }

        public string Nazwisko
        {
            get
            {
                return item.Nazwisko;
            }
            set
            {
                item.Nazwisko = value;
                OnPropertyChanged(() => Nazwisko);
            }
        }

        public string Ulica
        {
            get
            {
                return item.Ulica;
            }
            set
            {
                item.Ulica = value;
                OnPropertyChanged(() => Ulica);
            }
        }

        public string NrDomu
        {
            get
            {
                return item.NrDomu;
            }
            set
            {
                item.NrDomu = value;
                OnPropertyChanged(() => NrDomu);
            }
        }

        public string NrLokalu
        {
            get
            {
                return item.NrLokalu;
            }
            set
            {
                item.NrLokalu = value;
                OnPropertyChanged(() => NrLokalu);
            }
        }

        public string KodPocztowy
        {
            get
            {
                return item.KodPocztowy;
            }
            set
            {
                item.KodPocztowy = value;
                OnPropertyChanged(() => KodPocztowy);
            }
        }

        public string Miasto
        {
            get
            {
                return item.Miasto;
            }
            set
            {
                item.Miasto = value;
                OnPropertyChanged(() => Miasto);
            }
        }

        public int IdKraju
        {
            get
            {
                return item.IdKraju;
            }
            set
            {
                item.IdKraju = value;
                OnPropertyChanged(() => IdKraju);
            }
        }

        public string Email
        {
            get
            {
                return item.Email;
            }
            set
            {
                item.Email = value;
                OnPropertyChanged(() => Email);
            }
        }

        public string Telefon
        {
            get
            {
                return item.Telefon;
            }
            set
            {
                item.Telefon = value;
                OnPropertyChanged(() => Telefon);
            }
        }

        public string NIP
        {
            get
            {
                return item.NIP;
            }
            set
            {
                item.NIP = value;
                OnPropertyChanged(() => NIP);
            }
        }
        #endregion

        #region Items
        public IEnumerable<KeyAndValue> KrajItems
        {
            get
            {
                return new KrajB(db).GetKrajKeyAndValueItems();
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdKlienta == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.Klient.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.Klient.FirstOrDefault(f => f.IdKlienta == item.IdKlienta);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("KlientRefresh");
        }
        #endregion

        #region Constructor
        public NowyKlientViewModel()
            : base("Klient")
        {
            db = new HotelEntities();
            item = new Klient();
        }

        public NowyKlientViewModel(int itemId)
            : base("Edycja klienta")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.Klient.FirstOrDefault(k => k.IdKlienta == itemId);

            if (item != null)
            {
                Imie = item.Imie;
                Nazwisko = item.Nazwisko;
                Ulica = item.Ulica;
                NrDomu = item.NrDomu;
                NrLokalu = item.NrLokalu;
                KodPocztowy = item.KodPocztowy;
                Miasto = item.Miasto;
                IdKraju = item.IdKraju;
                Email = item.Email;
                Telefon = item.Telefon;
                NIP = item.NIP;
            }
        }
        #endregion

        #region Validation
        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Imie):
                    return StringValidator.ContainsOnlyLettersWithSpaces(Imie) ? "wprowadź poprawne imie (nie używaj cyfr ani znaków specjalnych)" : string.Empty;

                case nameof(Nazwisko):
                    return StringValidator.ContainsOnlyLettersWithSpaces(Nazwisko) ? "wprowadź poprawne nazwisko (nie używaj cyfr ani znaków specjalnych)" : string.Empty;

                case nameof(Ulica):
                    return StringValidator.IsValidStreet(Ulica) ? "wprowadź poprawną nazwę ulicy" : string.Empty;

                case nameof(NrDomu):
                    return StringValidator.IsValidHouseNumber(NrDomu) ? "wprowadź poprawny numer domu" : string.Empty;

                case nameof(NrLokalu):
                    // lokal jest opcjonalny
                    if (string.IsNullOrEmpty(NrLokalu))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return !StringValidator.IsPositiveNumber(NrLokalu) ? "wprowadź poprawny numer lokalu lub zostaw puste" : string.Empty;
                    }

                case nameof(KodPocztowy):
                    return StringValidator.IsValidPostalCode(KodPocztowy) ? "wprowadź poprawny kod pocztowy w formacie XX-XXX" : string.Empty;

                case nameof(Miasto):
                    return StringValidator.IsValidCity(Miasto) ? "wprowadź poprawną nazwę miasta" : string.Empty;

                case nameof(IdKraju):
                    return IdKraju <= 0 ? "wybierz kraj" : string.Empty;

                case nameof(Email):
                    // email jest opcjonalny
                    if (string.IsNullOrEmpty(Email))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return StringValidator.IsValidEmail(Email) ? "wprowadź poprawny adres email lub zostaw puste" : string.Empty;
                    }

                case nameof(Telefon):
                    return StringValidator.IsValidPhoneNumber(Telefon) ? "wprowadź poprawny numer telefonu (bez numeru kierunkowego)" : string.Empty;

                case nameof(NIP):
                    if (string.IsNullOrEmpty(NIP))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return StringValidator.IsValidNIP(NIP) ? "wprowadź poprawny NIP (10 cyfr) lub zostaw puste" : string.Empty;
                    }

                default:
                    return string.Empty;

            }
        }

        public string ValidateDuplicate()
        {
            var istniejacyRekord = db.Klient.FirstOrDefault(k =>
                k.Imie == Imie &&
                k.Nazwisko == Nazwisko &&
                k.Ulica == Ulica &&
                k.NrDomu == NrDomu &&
                k.NrLokalu == NrLokalu &&
                k.KodPocztowy == KodPocztowy &&
                k.Miasto == Miasto &&
                k.IdKraju == IdKraju &&
                k.Email == Email &&
                k.Telefon == Telefon &&
                k.NIP == NIP &&
                k.IdKlienta != item.IdKlienta);

            if (istniejacyRekord != null)
                return $"istnieje już klient o tych samych danych. Ma ID {istniejacyRekord.IdKlienta}. Możesz go edytować lub usunąć i dodać na nowo.";

            return string.Empty;
        }

        #endregion
    }
}
