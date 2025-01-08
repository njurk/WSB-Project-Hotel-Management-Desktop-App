using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class NowyFakturaViewModel : JedenViewModel<Faktura>
    {
        #region Fields
        private decimal _stawkaVat;
        #endregion

        #region DB
        HotelEntities db;
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
            IdVat = DomyslnyVAT("23");

            SelectedRezerwacja = new Rezerwacja();
        }

        public NowyFakturaViewModel(int itemId)
            : base("Edycja faktury")
        {
            db = new HotelEntities();
            item = db.Faktura.FirstOrDefault(f => f.IdFaktury == itemId);

            if (item != null)
            {
                NrFaktury = item.NrFaktury;
                IdRezerwacji = item.IdRezerwacji;
                DataWystawienia = item.DataWystawienia;
                DataSprzedazy = item.DataSprzedazy;
                KwotaBrutto = item.KwotaBrutto;
                IdVat = item.IdVat;
                KwotaNetto = item.KwotaNetto;
                TerminPlatnosci = item.TerminPlatnosci;
                Opis = item.Opis;

                SelectedRezerwacja = db.Rezerwacja.FirstOrDefault(r => r.IdRezerwacji == item.IdRezerwacji);
            }
        }
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
                    // wybrana rezerwacja przekazywana do metody aby odczytać i wstawić
                    // jej dane do pól - usprawnienie procesu dodawania
                    SelectedRezerwacja = db.Rezerwacja.FirstOrDefault(r => r.IdRezerwacji == value);
                }
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

                    // konwersja wybranej stawki vat na decimal jest potrzebna
                    // dla metody ObliczNetto
                    var vatItem = VATItems.FirstOrDefault(v => v.Key == IdVat);
                    _stawkaVat = vatItem != null ? Convert.ToDecimal(vatItem.Value) : 0;
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

        private Rezerwacja _selectedRezerwacja;
        // ta właściwość pobiera z wybranej rezerwacji konkretne dane
        // i ustawia w polach np. data sprzedaży, kwota brutto
        // celem usprawnienia procesu dodawania faktury
        
        public Rezerwacja SelectedRezerwacja
        {
            get { return _selectedRezerwacja; }
            set
            {
                if (_selectedRezerwacja != value)
                {
                    _selectedRezerwacja = value;
                    OnPropertyChanged(() => SelectedRezerwacja);
                    if (_selectedRezerwacja != null)
                    {
                        // usatwienie daty sprzedaży
                        DataSprzedazy = _selectedRezerwacja.DataZameldowania;
                        OnPropertyChanged(() => DataSprzedazy);

                        // usatwienie kwoty brutto
                        var rezerwacja = db.Rezerwacja.FirstOrDefault(p => p.IdRezerwacji == _selectedRezerwacja.IdRezerwacji);
                        if (rezerwacja != null)
                        {
                            KwotaBrutto = rezerwacja.Kwota;
                            OnPropertyChanged(() => KwotaBrutto);
                        }
                        // ustawienie kwoty do zapłacenia
                        SumaPlatnosci = sumaPlatnosci(_selectedRezerwacja.IdRezerwacji);
                    }
                }
            }
        }
        //metoda do obliczania kwoty pozostałej do zapłacenia dla danej rezerwacji
        private decimal sumaPlatnosci(int idRezerwacji)
        {
            return db.Platnosc
                     .Where(p => p.IdRezerwacji == idRezerwacji)
                     .Sum(p => (decimal?)p.Kwota) ?? 0;
        }

        private decimal _sumaPlatnosci;

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

        public List<KeyAndValue> RezerwacjaItems
        {
            get
            {
                // sprawdzenie czy jesteśmy w edycji
                if (item.IdFaktury > 0)
                {
                    // dodanie do combobox numeru rezerwacji z faktury która jest aktualnie edytowana
                    var rezerwacja = db.Rezerwacja
                                       .FirstOrDefault(r => r.IdRezerwacji == item.IdRezerwacji);

                    // dodanie do combobox wszystkich rezerwacji bez faktury
                    var rezerwacjeBezFaktury = db.Rezerwacja
                        .Where(r => !db.Faktura.Any(f => f.IdRezerwacji == r.IdRezerwacji))
                        .Select(r => new KeyAndValue
                        {
                            Key = r.IdRezerwacji,
                            Value = r.NrRezerwacji
                        }).ToList();

                    if (rezerwacja != null)
                    {
                        var selectedRezerwacja = new KeyAndValue
                        {
                            Key = rezerwacja.IdRezerwacji,
                            Value = rezerwacja.NrRezerwacji
                        };
                        // numer rezerwacji z faktury aktualnie edytowanej jako pierwszy na liście
                        rezerwacjeBezFaktury.Insert(0, selectedRezerwacja);
                    }

                    return rezerwacjeBezFaktury;
                }
                else
                {
                    // jesteśmy w dodawaniu nowej faktury
                    // w combobox znajdą się do wyboru tylko rezerwacje bez faktury
                    return db.Rezerwacja
                        .Where(r => !db.Faktura.Any(f => f.IdRezerwacji == r.IdRezerwacji))
                        .Select(r => new KeyAndValue
                        {
                            Key = r.IdRezerwacji,
                            Value = r.NrRezerwacji
                        }).ToList();
                }
            }
        }

        public IQueryable<KeyAndValue> VATItems
        {
            get
            {
                return new VATB(db).GetVATKeyAndValueItems();
            }
        }
        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(NrFaktury):
                    return string.IsNullOrEmpty(NrFaktury) ? "Proszę wprowadzić numer faktury" : string.Empty;

                case nameof(IdRezerwacji):
                    return IdRezerwacji <= 0 ? "Proszę wybrać rezerwację" : string.Empty;

                case nameof(DataWystawienia):
                    return DataWystawienia == null ? "Data wystawienia nie może być pusta" : string.Empty;

                case nameof(DataSprzedazy):
                    return DataSprzedazy == null ? "Data sprzedaży nie może być pusta" : string.Empty;

                case nameof(KwotaBrutto):
                    return KwotaBrutto <= 0 ? "Proszę wprowadzić poprawną kwotę brutto" : string.Empty;

                case nameof(IdVat):
                    return IdVat <= 0 ? "Proszę wybrać stawkę VAT" : string.Empty;

                case nameof(KwotaNetto):
                    return KwotaNetto <= 0 ? "Proszę wprowadzić poprawną kwotę netto" : string.Empty;

                case nameof(TerminPlatnosci):
                    return TerminPlatnosci < DataWystawienia ? "Termin płatności nie może być wcześniej od daty wystawienia faktury" : string.Empty;

                default:
                    return string.Empty;
            }
        }
        #endregion

        #region Methods
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

        private BaseCommand _obliczNettoCommand;

        public BaseCommand ObliczNettoCommand
        {
            get
            {
                if (_obliczNettoCommand == null)
                {
                    _obliczNettoCommand = new BaseCommand(ObliczNetto);
                }
                return _obliczNettoCommand;
            }
        }

        private void ObliczNetto()
        {
            if (KwotaBrutto <= 0 || IdVat == -1)
            {
                MessageBox.Show("Niepoprawna kwota brutto lub nie wybrano stawki VAT", "Błąd", MessageBoxButton.OK);
                return;
            }

            KwotaNetto = Math.Round(_stawkaVat == 0 ? KwotaBrutto : KwotaBrutto / (1 + (_stawkaVat / 100)), 2);
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
            if (item.IdFaktury == 0) // brak ID = insert
            {
                db.Faktura.Add(item);
            }
            else // istnieje ID = update
            {
                var doEdycji = db.Faktura.FirstOrDefault(f => f.IdFaktury == item.IdFaktury);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // automatyczne odświeżenie listy po edycji rekordu
            Messenger.Default.Send("FakturaRefresh");
        }
        #endregion
    }
}