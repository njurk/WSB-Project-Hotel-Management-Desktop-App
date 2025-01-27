using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class NowyFakturaViewModel : JedenViewModel<Faktura>
    {
        #region Fields
        private BaseCommand _openRezerwacjeModalne;
        private string _nrRezerwacji;
        private decimal _stawkaVat;
        private decimal _sumaPlatnosci;
        #endregion

        #region DB
        HotelEntities db;
        #endregion

        #region Properties
        public string NrFaktury
        {
            get
            {
                return item.NrFaktury;
            }
            set
            {
                item.NrFaktury = value;
                OnPropertyChanged(() => NrFaktury);
            }
        }

        public string NrRezerwacji
        {
            get
            {
                return _nrRezerwacji;
            }
            set
            {
                _nrRezerwacji = value;
                OnPropertyChanged(() => NrRezerwacji);
            }
        }

        public int IdRezerwacji
        {
            get
            {
                return item.IdRezerwacji;
            }
            set
            {
                if (item.IdRezerwacji != value)
                {
                    item.IdRezerwacji = value;
                    OnPropertyChanged(() => IdRezerwacji);

                    var rezerwacja = db.Rezerwacja.FirstOrDefault(r => r.IdRezerwacji == value);
                    if (rezerwacja != null)
                    {
                        NrRezerwacji = rezerwacja.NrRezerwacji;
                        DataSprzedazy = rezerwacja.DataZameldowania;
                        KwotaBrutto = rezerwacja.Kwota;
                        SumaPlatnosci = sumaPlatnosci(rezerwacja.IdRezerwacji);
                        NIP = rezerwacja.Klient?.NIP ?? string.Empty;

                        OnPropertyChanged(() => NrRezerwacji);
                        OnPropertyChanged(() => DataSprzedazy);
                        OnPropertyChanged(() => KwotaBrutto);
                        OnPropertyChanged(() => SumaPlatnosci);
                        OnPropertyChanged(() => NIP);
                    }
                }
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
        public DateTime DataWystawienia
        {
            get
            {
                return item.DataWystawienia;
            }
            set
            {
                item.DataWystawienia = value;
                OnPropertyChanged(() => DataWystawienia);
            }
        }

        public DateTime DataSprzedazy
        {
            get
            {
                return item.DataSprzedazy;
            }
            set
            {
                if (item.DataSprzedazy != value)
                {
                    item.DataSprzedazy = value;
                    OnPropertyChanged(() => DataSprzedazy);
                }
            }
        }

        public decimal KwotaNetto
        {
            get
            {
                return item.KwotaNetto;
            }
            set
            {
                item.KwotaNetto = value;
                OnPropertyChanged(() => KwotaNetto);
            }
        }

        public int IdVat
        {
            get { return item.IdVat; }
            set
            {
                if (item.IdVat != value)
                {
                    item.IdVat = value;
                    OnPropertyChanged(() => IdVat);

                    // Konwersja wybranej stawki VAT na decimal jest potrzebna
                    // dla metody ObliczNetto
                    var vatItem = VATItems.FirstOrDefault(v => v.Key == IdVat);
                    _stawkaVat = vatItem != null ? Convert.ToDecimal(vatItem.Value) : 0;

                    ObliczNetto();
                }
            }
        }


        public decimal KwotaBrutto
        {
            get
            {
                return item.KwotaBrutto;
            }
            set
            {
                item.KwotaBrutto = value;
                OnPropertyChanged(() => KwotaBrutto);
                ObliczNetto();
            }
        }

        public DateTime TerminPlatnosci
        {
            get
            {
                return item.TerminPlatnosci;
            }
            set
            {
                item.TerminPlatnosci = value;
                OnPropertyChanged(() => TerminPlatnosci);
            }
        }
        public string Opis
        {
            get
            {
                return item.Opis;
            }
            set
            {
                item.Opis = value;
                OnPropertyChanged(() => Opis);
            }
        }

        public decimal SumaPlatnosci
        {
            get
            {
                return _sumaPlatnosci;
            }
            set
            {
                if (_sumaPlatnosci != value)
                {
                    _sumaPlatnosci = value;
                    OnPropertyChanged(() => SumaPlatnosci);
                }
            }
        }
        #endregion

        #region Items
        public IEnumerable<KeyAndValue> VATItems
        {
            get
            {
                return new VATB(db).GetVATKeyAndValueItems();
            }
        }
        #endregion

        #region Commands
        // komenda do wywołania metody otwierającej okienko modalne
        public BaseCommand OpenRezerwacjeModalneCommand
        {
            get
            {
                if (_openRezerwacjeModalne == null)
                {
                    _openRezerwacjeModalne = new BaseCommand(OpenRezerwacjeModalne);
                }
                return _openRezerwacjeModalne;
            }
        }
        #endregion

        #region Methods
        private void OpenRezerwacjeModalne()
        {
            var rezerwacjeModalne = new RezerwacjeModalneView
            {
                DataContext = new RezerwacjeModalneViewModel(true)
            };
            rezerwacjeModalne.ShowDialog();
        }

        //metoda do obliczania kwoty pozostałej do zapłacenia dla danej rezerwacji
        private decimal sumaPlatnosci(int idRezerwacji)
        {
            return db.Platnosc
                     .Where(p => p.IdRezerwacji == idRezerwacji)
                     .Sum(p => (decimal?)p.Kwota) ?? 0;
        }
        private int DomyslnyVAT(string szukanyVAT)
        {
            var domyslnyVAT = VATItems.FirstOrDefault(v => v.Value == szukanyVAT);

            if (domyslnyVAT != null)
            {
                return domyslnyVAT.Key;
            }
            else
            {
                return -1;
            }
        }

        // metoda obliczająca kwotę netto na podstawie kwoty rezerwacji i wybranego VAT
        private void ObliczNetto()
        {
            if (KwotaBrutto > 0 && IdVat > 0)
            {
                KwotaNetto = Math.Round(_stawkaVat == 0 ? KwotaBrutto : KwotaBrutto / (1 + (_stawkaVat / 100)), 2);
            }
        }

        private string GenerujNumerFaktury()
        {
            // pobranie ostatniej faktury z BD do ustalenia następnego numeru
            var ostatniaFaktura = db.Faktura
                                     .OrderByDescending(f => f.IdFaktury)
                                     .Select(f => f.NrFaktury)
                                     .FirstOrDefault();

            string numerFaktury;

            if (ostatniaFaktura != null)
            {
                string fakturaMiesiac = ostatniaFaktura.Substring(5, 2); // yyyy-MM
                string obecnyMiesiac = DateTime.Now.ToString("MM");

                if (fakturaMiesiac == obecnyMiesiac)
                {
                    int pozycjaF = ostatniaFaktura.IndexOf('F');
                    if (pozycjaF != -1 && pozycjaF + 1 < ostatniaFaktura.Length)
                    {
                        string numer = ostatniaFaktura.Substring(pozycjaF + 1);

                        if (int.TryParse(numer, out int numerInt))
                        {
                            numerFaktury = (numerInt + 1).ToString();
                        }
                        else
                        {
                            numerFaktury = "1";
                        }
                    }
                    else
                    {
                        numerFaktury = "1";
                    }
                }
                else
                {
                    numerFaktury = "1";
                }
            }
            else
            {
                numerFaktury = "1";
            }
            return $"{DateTime.Now:yyyy-MM}-F{numerFaktury}";
        }

        #endregion

        #region Helpers
        
        public override void Save()
        {
            if (item.IdFaktury == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.Faktura.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.Faktura.FirstOrDefault(f => f.IdFaktury == item.IdFaktury);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("FakturaRefresh");
        }
        #endregion

        #region Constructor
        public NowyFakturaViewModel()
            : base("Faktura")
        {
            db = new HotelEntities();
            item = new Faktura();
            DataWystawienia = DateTime.Now;
            DataSprzedazy = DateTime.Now;
            TerminPlatnosci = DateTime.Now.AddDays(14);
            NrFaktury = GenerujNumerFaktury();
            IdVat = DomyslnyVAT("23"); // metoda szuka w naszej bazie danych wartości "23" i ustawia ją na domyślną, dzięki czemu nie trzeba znać ID w bazie

            Messenger.Default.Register<int>(this, idRezerwacji => IdRezerwacji = idRezerwacji);
        }

        public NowyFakturaViewModel(int itemId)
            : base("Edycja faktury")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.Faktura.FirstOrDefault(f => f.IdFaktury == itemId);

            if (item != null)
            {
                NrFaktury = item.NrFaktury;
                NrRezerwacji = item.Rezerwacja.NrRezerwacji;
                IdRezerwacji = item.IdRezerwacji;
                NIP = item.NIP;
                DataWystawienia = item.DataWystawienia;
                DataSprzedazy = item.DataSprzedazy;
                KwotaBrutto = item.KwotaBrutto;
                IdVat = item.IdVat;
                KwotaNetto = item.KwotaNetto;
                TerminPlatnosci = item.TerminPlatnosci;
                Opis = item.Opis;
            }
            Messenger.Default.Register<int>(this, idRezerwacji => IdRezerwacji = idRezerwacji);
        }
        #endregion

        #region Validation
        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(NrRezerwacji):
                    return NrRezerwacji == null ? "wybierz rezerwację" : string.Empty;

                case nameof(NIP):
                    if (string.IsNullOrEmpty(NIP))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return StringValidator.IsValidNIP(NIP) ? "wprowadź poprawny NIP (10 cyfr) lub zostaw puste" : string.Empty;
                    }

                case nameof(DataWystawienia):
                    return (DataWystawienia == null || DataWystawienia > DateTime.Now) ? "wprowadź poprawną datę" : string.Empty;

                case nameof(DataSprzedazy):
                    return DataSprzedazy == null ? "wybierz poprawną datę sprzedaży faktury" : string.Empty;

                case nameof(KwotaBrutto):
                    return KwotaBrutto <= 0 ? "niepoprawna kwota brutto" : string.Empty;

                case nameof(IdVat):
                    return IdVat <= 0 ? "wybierz stawkę VAT" : string.Empty;

                case nameof(KwotaNetto):
                return KwotaNetto <= 0 ? "niepoprawna kwota netto" : string.Empty;

                case nameof(TerminPlatnosci):
                    return (TerminPlatnosci == null || TerminPlatnosci < DataWystawienia) ? "termin płatności musi być poźniej od daty wystawienia" : string.Empty;

                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}