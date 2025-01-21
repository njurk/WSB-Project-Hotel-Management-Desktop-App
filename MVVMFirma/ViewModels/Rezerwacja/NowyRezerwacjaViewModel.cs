using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class NowyRezerwacjaViewModel : JedenViewModel<Rezerwacja>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Fields
        private string _imieNazwiskoKlienta;
        private DateTime _dataZameldowania;
        private DateTime _dataWymeldowania;
        private Pokoj _selectedPokoj;
        private BaseCommand _openKlienciModalne;
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

        // do wyświetlania imienia i nazwiska zamiast IdRezerwacji w textboxie na widoku
        public string ImieNazwiskoKlienta
        {
            get => _imieNazwiskoKlienta;
            set
            {
                _imieNazwiskoKlienta = value;
                OnPropertyChanged(() => ImieNazwiskoKlienta);
            }
        }

        public int IdKlienta
        {
            get => item.IdKlienta;
            set
            {
                item.IdKlienta = value;
                OnPropertyChanged(() => IdKlienta);

                var klient = db.Klient.FirstOrDefault(k => k.IdKlienta == value);
                ImieNazwiskoKlienta = klient != null ? $"{klient.Imie} {klient.Nazwisko}" : string.Empty;
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
                // po każdej zmianie pokoju aktualizacja
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
                // należy wywołać walidacje pokoju przy każdej zmianie w liczbie dorosłych i dzieci
                // inaczej MaxGuestsValidator nie dowie się o zmianach w tych polach jeśli pokój był wybrany najpierw
                OnPropertyChanged(() => IdPokoju);
                OnPropertyChanged(() => SelectedPokoj);
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
                OnPropertyChanged(() => IdPokoju);
                OnPropertyChanged(() => SelectedPokoj);
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

        // aktualizacja listy dostępnych pokoi przy wybraniu dat
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

        #region Methods
        // komenda do wywołania metody otwierającej okienko modalne
        public BaseCommand OpenKlienciModalneCommand
        {
            get
            {
                if (_openKlienciModalne == null)
                {
                    _openKlienciModalne = new BaseCommand(OpenKlienciModalne);
                }
                return _openKlienciModalne;
            }
        }

        private void OpenKlienciModalne()
        {
            var klienciModalne = new KlienciModalneView();
            klienciModalne.ShowDialog();
        }

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

        // metoda do obliczania całkowitej kwoty rezerwacji na podstawie długości pobytu, liczby gości, posiadania zwierząt, wybranego pokoju oraz ewentualnej zniżki
        private void ObliczKwote()
        {
            // pobranie liczby dorosłych i dzieci
            int dorosli = int.Parse(LiczbaDoroslych);
            int dzieci = string.IsNullOrEmpty(LiczbaDzieci) ? 0 : int.Parse(LiczbaDzieci);

            // długość pobytu
            int liczbaNocy = (DataWymeldowania - DataZameldowania).Days;

            var cennik = db.Cennik.FirstOrDefault(c => c.IdKlasyPokoju == SelectedPokoj.IdKlasyPokoju && c.IdTypuPokoju == SelectedPokoj.IdTypuPokoju);
            // sprawdzenie czy istnieje cennik dla pary typu i klasy wybranego pokoju
            if (cennik == null)
            {
                MessageBox.Show("nie znaleziono cennika dla parametrów wybranego pokoju, utwórz go w zakładce Cenniki", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // kwota podstawowa za pokój, za jedną noc
            decimal kwota = Convert.ToInt32(SelectedPokoj.TypPokoju.MaxLiczbaOsob) * cennik.CenaDorosly;

            // obliczanie kwoty uwzględniając dzieci
            kwota -= (dzieci * cennik.CenaDorosly);
            kwota += (dzieci * cennik.CenaDziecko);

            // cała kwota z doliczeniem ewentualnej opłaty za zwierzęta
            decimal kwotaCalkowita = kwota * liczbaNocy + (CzyZwierzeta ? cennik.CenaZwierzeta * liczbaNocy : 0);

            // zastosowanie zniżki jeśli wybrana
            if (IdZnizki.HasValue)
            {
                var znizka = db.Znizka.FirstOrDefault(z => z.IdZnizki == IdZnizki.Value);
                if (znizka != null) kwotaCalkowita *= 1 - Convert.ToInt32(znizka.Wartosc) / 100m;
            }

            // kwota finalna
            Kwota = kwotaCalkowita;
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            ObliczKwote();

            if (item.IdRezerwacji == 0) // dodawanie rekordu = brak ID = insert
            {
                db.Rezerwacja.Add(item);
            }
            else // edycja rekordu = istnieje ID = update
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

            Messenger.Default.Register<int>(this, idKlienta => IdKlienta = idKlienta);
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
            Messenger.Default.Register<int>(this, idKlienta => IdKlienta = idKlienta);
        }
        #endregion

        #region Validation
        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ImieNazwiskoKlienta):
                    return ImieNazwiskoKlienta == null ? "wybierz klienta" : string.Empty;

                case nameof(LiczbaDoroslych):
                    return !StringValidator.IsPositiveNumber(LiczbaDoroslych) ? "wprowadź poprawną liczbę dorosłych" : string.Empty;

                case nameof(LiczbaDzieci):
                    return !(string.IsNullOrWhiteSpace(LiczbaDzieci) || StringValidator.IsPositiveNumber(LiczbaDzieci)) ? "wprowadź poprawną liczbę dzieci (lub zostaw puste)" : string.Empty;

                case nameof(DataZameldowania):
                    if (DataZameldowania >= DataWymeldowania)
                        return "data zameldowania musi być wcześniej od daty wymeldowania";
                    if (DataZameldowania < DateTime.Now.Date)
                        return "data zameldowania nie może być w przeszłości";
                    return string.Empty;

                case nameof(DataWymeldowania):
                    return DataWymeldowania <= DataZameldowania ? "data wymeldowania musi być później od daty zameldowania" : string.Empty;

                case nameof(DataRezerwacji):
                    return DataRezerwacji > DateTime.Now ? "data rezerwacji nie może być w przyszłości" : string.Empty;

                case nameof(IdPokoju):
                    if (IdPokoju == 0)
                    {
                        return "wybierz pokój";
                    }

                    if (!int.TryParse(LiczbaDoroslych, out int dorosli) || dorosli <= 0)
                    {
                        return "wprowadź poprawną liczbę dorosłych";
                    }

                    // dzieci są opcjonalne w rezerwacji więc sprawdzamy tylko gdy wartość nie jest null
                    int liczbaDzieci = string.IsNullOrWhiteSpace(LiczbaDzieci) ? 0 : int.Parse(LiczbaDzieci);

                    string maxGuestsError = new MaxGuestsValidator(db).Validate(IdPokoju, dorosli, liczbaDzieci);
                    return !string.IsNullOrEmpty(maxGuestsError) ? maxGuestsError : string.Empty;

                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}
