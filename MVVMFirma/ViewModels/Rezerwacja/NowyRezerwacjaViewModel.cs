using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;

namespace MVVMFirma.ViewModels
{
    public class NowyRezerwacjaViewModel : JedenViewModel<Rezerwacja>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Fields
        private DateTime _dataZameldowania;
        private DateTime _dataWymeldowania;
        private Pokoj _selectedPokoj;
        private BaseCommand _obliczKwoteCommand;

        #endregion

        #region Properties
        public string NrRezerwacji
        {
            get
            {
                return item.NrRezerwacji;
            }
            set
            {
                item.NrRezerwacji = value;
                OnPropertyChanged(() => NrRezerwacji);
            }
        }
        public int IdKlienta
        {
            get
            {
                return item.IdKlienta;
            }
            set
            {
                item.IdKlienta = value;
                OnPropertyChanged(() => IdKlienta);
            }
        }

        public int IdPokoju
        {
            get 
            { 
                return item.IdPokoju; 
            }
            set
            {
                item.IdPokoju = value;
                OnPropertyChanged(() => IdPokoju);
                // po każdej zmianie pokoju aktualizacja zaznaczenia
                SelectedPokoj = db.Pokoj.FirstOrDefault(p => p.IdPokoju == value);
            }
        }

        public string LiczbaDoroslych
        {
            get
            {
                return item.LiczbaDoroslych;
            }
            set
            {
                item.LiczbaDoroslych = value;
                OnPropertyChanged(() => LiczbaDoroslych);
            }
        }

        public string LiczbaDzieci
        {
            get
            {
                return item.LiczbaDzieci;
            }
            set
            {
                item.LiczbaDzieci = value;
                OnPropertyChanged(() => LiczbaDzieci);
            }
        }

        public bool CzyZwierzeta
        {
            get
            {
                return item.CzyZwierzeta;
            }
            set
            {
                item.CzyZwierzeta = value;
                OnPropertyChanged(() => CzyZwierzeta);
            }
        }
        public DateTime DataZameldowania
        {
            get 
            { 
                return _dataZameldowania; 
            }
            set
            {
                item.DataZameldowania = value;
                _dataZameldowania = value;
                OnPropertyChanged(() => DataZameldowania);
                OnDataChanged();
            }
        }
        public DateTime DataWymeldowania
        {
            get 
            { 
                return _dataWymeldowania; 
            }
            set
            {
                item.DataWymeldowania = value;
                _dataWymeldowania = value;
                OnPropertyChanged(() => DataWymeldowania);
                OnDataChanged();
            }
        }

        // aktualizacja listy pokoi przy wybraniu dat
        private void OnDataChanged()
        {
            if (DataZameldowania != null && DataWymeldowania != null)
            {
                OnPropertyChanged(() => PokojItems); 
            }
        }

        public DateTime DataRezerwacji
        {
            get
            {
                return item.DataRezerwacji;
            }
            set
            {
                item.DataRezerwacji = value;
                OnPropertyChanged(() => DataRezerwacji);
            }
        }

        public string Uwagi
        {
            get
            {
                return item.Uwagi;
            }
            set
            {
                item.Uwagi = value;
                OnPropertyChanged(() => Uwagi);
            }
        }

        public int? IdZnizki
        {
            get
            {
                return item.IdZnizki;
            }
            set
            {
                item.IdZnizki = value;
                OnPropertyChanged(() => IdZnizki);
            }
        }

        public bool CzyZnizka
        {
            get
            {
                return item.IdZnizki.HasValue;
            }
            set
            {
                if (value)
                {
                    if (IdZnizki == null)
                    {
                        IdZnizki = ZnizkaItems.FirstOrDefault()?.Key;
                    }
                }
                else
                {
                    IdZnizki = null;
                }
                OnPropertyChanged(() => CzyZnizka);
            }
        }
        public decimal Kwota
        {
            get
            {
                return item.Kwota;
            }
            set
            {
                item.Kwota = value;
                OnPropertyChanged(() => Kwota);
            }
        }

        // poniższa właściwość ustawia pokój jako wybrany element w comboboxie przy edycji, dzięki czemu jest widoczny od razu
        public Pokoj SelectedPokoj
        {
            get { return _selectedPokoj; }
            set
            {
                if (_selectedPokoj != value)
                {
                    _selectedPokoj = value;
                    OnPropertyChanged(() => SelectedPokoj);

                    if (_selectedPokoj != null)
                    {
                        IdPokoju = _selectedPokoj.IdPokoju;
                    }
                }
            }
        }
        #endregion

        #region Items
        public IEnumerable<KeyAndValue> KlientItems
        {
            get
            {
                return new KlientB(db).GetKlientKeyAndValueItems();
            }
        }
        public IEnumerable<KeyAndValue> PokojItems
        {
            get
            {
                var dostepnePokoje = db.Pokoj.Where(p =>
                    !db.Rezerwacja.Any(r =>
                        r.IdPokoju == p.IdPokoju &&
                        r.DataZameldowania < DataWymeldowania &&
                        r.DataWymeldowania > DataZameldowania))
                    .Select(p => new KeyAndValue
                    {
                        Key = p.IdPokoju,
                        Value = p.NrPokoju + " - " + p.TypPokoju.Nazwa + " " + p.KlasaPokoju.Nazwa
                    }).ToList();

                if (item.IdRezerwacji != 0)  // tryb edycji - pobranie pokoju do comboboxa
                {
                    var aktualnyPokoj = db.Rezerwacja.FirstOrDefault(r => r.IdRezerwacji == item.IdRezerwacji)?.IdPokoju;
                    if (aktualnyPokoj.HasValue)
                    {
                        var pokoj = db.Pokoj.FirstOrDefault(p => p.IdPokoju == aktualnyPokoj.Value);
                        if (pokoj != null)
                        {
                            dostepnePokoje.Insert(0, new KeyAndValue { Key = pokoj.IdPokoju, Value = pokoj.NrPokoju + " - " + pokoj.TypPokoju.Nazwa + " " + pokoj.KlasaPokoju.Nazwa });
                        }
                    }
                }

                return new ObservableCollection<KeyAndValue>(dostepnePokoje);
            }
        }

        public IEnumerable<KeyAndValue> ZnizkaItems
        {
            get
            {
                return new ZnizkaB(db).GetZnizkaKeyAndValueItems();
            }
        }
        #endregion

        #region Validation
        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IdKlienta):
                    return IdKlienta <= 0 ? "Wybierz klienta" : string.Empty;

                case nameof(LiczbaDoroslych):
                    if (!int.TryParse(LiczbaDoroslych, out int dorosli) || dorosli <= 0)
                        return "Wprowadź poprawną liczbę dorosłych.";
                    return string.Empty;

                case nameof(LiczbaDzieci):
                    if (!string.IsNullOrWhiteSpace(LiczbaDzieci) && (!int.TryParse(LiczbaDzieci, out int dzieci) || dzieci < 0))
                        return "Wprowadź poprawną liczbę dzieci (lub pozostaw puste).";
                    return string.Empty;

                case nameof(IdPokoju):
                    if (IdPokoju > 0)
                    {
                        int liczbaDoroslych = 0, liczbaDzieci = 0;
                        // pobranie danych pokoju
                        var pokoj = db.Pokoj.FirstOrDefault(p => p.IdPokoju == IdPokoju);

                        // sprawdzenie max liczby osób
                        if (pokoj != null && (liczbaDoroslych + liczbaDzieci) > Convert.ToInt32(pokoj.TypPokoju.MaxLiczbaOsob))
                        {
                            return $"Łączna liczba osób ({liczbaDoroslych + liczbaDzieci}) przekracza maksymalny limit ({pokoj.TypPokoju.MaxLiczbaOsob}) dla wybranego pokoju.";
                        }
                    }
                    else
                    {
                        return "Wybierz pokój.";
                    }
                    return string.Empty;

                case nameof(DataZameldowania):
                    return DataZameldowania > DataWymeldowania ? "Data zameldowania nie może być późniejsza od daty wymeldowania." : string.Empty;

                case nameof(DataWymeldowania):
                    return DataWymeldowania < DataZameldowania ? "Data wymeldowania nie może poprzedzać daty zameldowania." : string.Empty;

                case nameof(DataRezerwacji):
                    return DataRezerwacji > DataZameldowania ? "Data rezerwacji nie może być późniejsza niż data zameldowania." : string.Empty;
                
                case nameof(Kwota):
                    return Kwota == 0 ? "Proszę obliczyć kwotę." : string.Empty;
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region Methods
        public string GenerujNumerRezerwacji()
        {
            // pobranie ostatniego numeru rezerwacji z BD jako punkt odniesienia
            var ostatniaRezerwacja = db.Rezerwacja
                .OrderByDescending(r => r.IdRezerwacji) 
                .Select(r => r.NrRezerwacji)
                .FirstOrDefault();

            // deklaracja zmiennej która otrzyma i na końcu ustawi odpowiedni numer aktualnie tworzonej rezerwacji
            string nrRezerwacji;

            // jeśli istnieje jakakolwiek rezerwacja w bazie
            if (ostatniaRezerwacja != null)
            {
                string rezerwacjaMiesiac = ostatniaRezerwacja.Substring(5, 2); // yyyy-MM
                string obecnyMiesiac = DateTime.Now.ToString("MM");

                if (rezerwacjaMiesiac == obecnyMiesiac)
                {
                    // ustalenie pozycji "R" w stringu aby wyodrębnić numer do inkrementacji
                    int pozycjaR = ostatniaRezerwacja.IndexOf('R');
                    if (pozycjaR != -1 && pozycjaR + 1 < ostatniaRezerwacja.Length)
                    {
                        string nr = ostatniaRezerwacja.Substring(pozycjaR + 1);

                        // próba konwersji stringu z numerem na int aby zwiększyć o 1
                        if (int.TryParse(nr, out int numerInt))
                        {
                            nrRezerwacji = (numerInt + 1).ToString();
                        }
                        else // jeśli nie udało się zamienić na int
                        {
                            nrRezerwacji = "1";
                        }
                    }
                    else // jeśli R nie znaleziona lub nie ma za nią żadnych cyfr
                    {
                        nrRezerwacji = "1";
                    }
                }
                else // jeśli jest nowy miesiąc
                {
                    nrRezerwacji = "1";
                }
            }
            else // jeśli w bazie nie ma jeszcze ani jednej rezerwacji
            {
                nrRezerwacji = "1";
            }
            return $"{DateTime.Now:yyyy-MM}-R{nrRezerwacji}";
        }

        // komenda wywołująca metodę ObliczKwote(), aby na widoku można było aktywować ją buttonem
        public BaseCommand ObliczKwoteCommand
        {
            get
            {
                if (_obliczKwoteCommand == null)
                {
                    _obliczKwoteCommand = new BaseCommand(ObliczKwote);
                }
                return _obliczKwoteCommand;
            }
        }

        // metoda do obliczania całkowitej kwoty rezerwacji na podstawie długości pobytu i wybranego pokoju oraz ewentualnej zniżki
        private void ObliczKwote()
        {
            if (SelectedPokoj == null || !int.TryParse(LiczbaDoroslych, out int liczbaDoroslych))
            {
                MessageBox.Show("Proszę wprowadzić poprawne dane we wszystkich polach.", "Błąd", MessageBoxButton.OK);
                return;
            }

            // liczba dzieci może być pusta lub 0 i będzie to poprawne
            // konwersja inputu na int
            int liczbaDzieci = string.IsNullOrEmpty(LiczbaDzieci) ? 0 : int.Parse(LiczbaDzieci);
            // długość pobytu obliczana przy użyciu wbudowanej funkcji Days()
            int liczbaNocy = (DataWymeldowania - DataZameldowania).Days;

            // pobranie odpowiedniego rekordu z tabeli Cennik (wg dwóch kluczy - klasy i typu pokoju) 
            var cennik = db.Cennik.FirstOrDefault(c => c.IdKlasyPokoju == SelectedPokoj.IdKlasyPokoju && c.IdTypuPokoju == SelectedPokoj.IdTypuPokoju);

            // jeśli brak cennika dla wybranych parametrów
            if (cennik == null)
            {
                MessageBox.Show("Nie znaleziono odpowiedniego cennika, dodaj go w zakładce Cenniki.", "Błąd", MessageBoxButton.OK);
                return;
            }

            decimal calkowitaKwota = ((liczbaDoroslych * cennik.CenaDorosly) + (liczbaDzieci * cennik.CenaDziecko) + (CzyZwierzeta ? cennik.CenaZwierzeta : 0)) * liczbaNocy;

            // używając funkcji HasValue sprawdzamy czy została wybrana zniżka, jeśli tak, to uwzględniamy w kwocie całkowitej
            if (IdZnizki.HasValue)
            {
                var znizka = db.Znizka.FirstOrDefault(z => z.IdZnizki == IdZnizki.Value);
                if (znizka != null)
                {
                    decimal wartoscZnizki = Convert.ToInt32(znizka.Wartosc) / 100m;
                    calkowitaKwota -= calkowitaKwota * wartoscZnizki;
                }
            }

            Kwota = calkowitaKwota;
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdRezerwacji == 0) // Dodawanie rekordu = brak ID = insert
            {
                db.Rezerwacja.Add(item);
            }
            else // Edycja rekordu = istnieje ID = update
            {
                var doEdycji = db.Rezerwacja.FirstOrDefault(f => f.IdRezerwacji == item.IdRezerwacji);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // wysłanie prośby o odświeżenie listy po zapisie
            Messenger.Default.Send("RezerwacjaRefresh");
        }
        #endregion

        #region Constructor
        public NowyRezerwacjaViewModel()
            : base("Rezerwacja")
        {
            db = new HotelEntities();
            item = new Rezerwacja();
            DataRezerwacji = DateTime.Now;
            DataZameldowania = DateTime.Now.AddDays(1);
            DataWymeldowania = DateTime.Now.AddDays(2);
            NrRezerwacji = GenerujNumerRezerwacji();
        }

        public NowyRezerwacjaViewModel(int itemId)
            : base("Edycja rezerwacji")
        {
            db = new HotelEntities();
            // inicjalizacja pól danymi z rekordu o ID przekazanym w argumencie (itemId)
            item = db.Rezerwacja.FirstOrDefault(r => r.IdRezerwacji == itemId);

            if (item != null)
            {
                NrRezerwacji = item.NrRezerwacji;
                IdKlienta = item.IdKlienta;
                IdPokoju = item.IdPokoju;
                LiczbaDoroslych = item.LiczbaDoroslych;
                LiczbaDzieci = item.LiczbaDzieci;
                CzyZwierzeta = item.CzyZwierzeta;
                DataZameldowania = item.DataZameldowania;
                DataWymeldowania = item.DataWymeldowania;
                DataRezerwacji = item.DataRezerwacji;
                Kwota = item.Kwota;
                Uwagi = item.Uwagi;
                // przekazanie pokoju do właściwości, która odpowiada za do ustawienie jej w comboboxie
                SelectedPokoj = db.Pokoj.FirstOrDefault(p => p.IdPokoju == item.IdPokoju);
            }
        }
        #endregion
    }
}
