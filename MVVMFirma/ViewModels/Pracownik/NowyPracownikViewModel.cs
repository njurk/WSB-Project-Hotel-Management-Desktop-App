using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyPracownikViewModel : JedenViewModel<Pracownik>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Properties
        public int IdStanowiska
        {
            get
            {
                return item.IdStanowiska;
            }
            set
            {
                item.IdStanowiska = value;
                OnPropertyChanged(() => IdStanowiska);
            }
        }

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

        public DateTime DataUrodzenia
        {
            get
            {
                return item.DataUrodzenia;
            }
            set
            {
                item.DataUrodzenia = value;
                OnPropertyChanged(() => DataUrodzenia);
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
        #endregion

        #region Items
        public IEnumerable<KeyAndValue> StanowiskoItems
        {
            get
            {
                return new StanowiskoB(db).GetStanowiskoKeyAndValueItems();
            }
        }

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
            if (item.IdPracownika == 0) // dodawanie rekordu = brak ID = insert
            {
                db.Pracownik.Add(item);
            }
            else // edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.Pracownik.FirstOrDefault(f => f.IdPracownika == item.IdPracownika);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("PracownikRefresh");
        }
        #endregion

        #region Constructor
        public NowyPracownikViewModel()
            : base("Pracownik")
        {
            db = new HotelEntities();
            item = new Pracownik();
            DataUrodzenia = DateTime.Now.AddYears(-30); // domyślna data 30 lat wstecz aby ułatwić wybór
        }

        public NowyPracownikViewModel(int itemId)
            : base("Edycja pracownika")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.Pracownik.FirstOrDefault(p => p.IdPracownika == itemId);

            if (item != null)
            {
                IdStanowiska = item.IdStanowiska;
                Imie = item.Imie;
                Nazwisko = item.Nazwisko;
                Ulica = item.Ulica;
                NrDomu = item.NrDomu;
                NrLokalu = item.NrLokalu;
                KodPocztowy = item.KodPocztowy;
                Miasto = item.Miasto;
                IdKraju = item.IdKraju;
                DataUrodzenia = item.DataUrodzenia;
                Email = item.Email;
                Telefon = item.Telefon;
            }
        }
        #endregion

        #region Validation
        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IdStanowiska):
                    return IdStanowiska <= 0 ? "wybierz stanowisko pracownika" : string.Empty;

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

                case nameof(DataUrodzenia):
                    return StringValidator.IsValidDateOfBirth(DataUrodzenia) ? "wybierz poprawną datę urodzenia" : string.Empty;

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

                default:
                    return string.Empty;
            
            }
        }
        #endregion
    }
}
