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

            _selectedRezerwacja = new Rezerwacja();

            string szukanyVAT = "23";
            var domyslnyVAT = VATItems.FirstOrDefault(v => v.Value == szukanyVAT);

            if (domyslnyVAT != null)
            {
                IdVat = domyslnyVAT.Key;
            }
            else
            {
                IdVat = -1;
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
                    // iteracja przez tabelę z BD w poszukiwaniu odpowiedniego
                    // IdRezerwacji i przekazanie go do SelectedRezerwacja
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
        // ta właściwość przechowuje wybraną rezerwację i ustawia powiązane z nią informacje 
        // do odpowiednich pól, wywołując onpropertychanged
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

                        // suma platnosci do wyświetlenia w textblocku
                        var sumaPlatnosci = db.Platnosc
                        .Where(p => p.IdRezerwacji == _selectedRezerwacja.IdRezerwacji)
                        .Sum(p => (decimal?)p.Kwota) ?? 0;
                        SumaPlatnosci = sumaPlatnosci;
                    }
                    else
                    {
                        SumaPlatnosci = 0;
                    }
                }
            }
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
                return db.Rezerwacja
                    .Where(r => !db.Faktura.Any(f => f.IdRezerwacji == r.IdRezerwacji))
                    .Select(r => new KeyAndValue
                    {
                        Key = r.IdRezerwacji,
                        Value = r.NrRezerwacji
                    }).ToList();
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
                                     .OrderByDescending(f => f.DataWystawienia)
                                     .ThenByDescending(f => f.IdFaktury) // sortowanie po ID jeśli daty są takie same
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
            // format RRRR-MM-FX
            return $"{DateTime.Now:yyyy-MM}-F{numerFaktury}";
        }

        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Faktura.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}