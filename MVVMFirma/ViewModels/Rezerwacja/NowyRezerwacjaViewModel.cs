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

        #region Constructor
        public NowyRezerwacjaViewModel()
            :base("Rezerwacja")
        {
            db = new HotelEntities();
            item = new Rezerwacja();
            DataRezerwacji = DateTime.Now;
            DataZameldowania = DateTime.Now.AddDays(1);
            DataWymeldowania = DateTime.Now.AddDays(2);
            NrRezerwacji = GenerujNumerRezerwacji();
        }

        public NowyRezerwacjaViewModel(int itemId)
            :base("Edycja rezerwacji")
        {
            db = new HotelEntities();
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
                SelectedPokoj = db.Pokoj.FirstOrDefault(p => p.IdPokoju == item.IdPokoju);
            }
        }
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
            get { return item.IdPokoju; }
            set
            {
                if (item.IdPokoju != value)
                {
                    item.IdPokoju = value;
                    OnPropertyChanged(() => IdPokoju);

                    SelectedPokoj = db.Pokoj.FirstOrDefault(p => p.IdPokoju == value);
                }
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

        private DateTime _dataZameldowania;
        public DateTime DataZameldowania
        {
            get { return _dataZameldowania; }
            set
            {
                item.DataZameldowania = value;
                _dataZameldowania = value;
                OnPropertyChanged(() => DataZameldowania);
                OnDataChanged();
            }
        }
        private DateTime _dataWymeldowania;
        public DateTime DataWymeldowania
        {
            get { return _dataWymeldowania; }
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

        public IQueryable<KeyAndValue> KlientItems
        {
            get
            {
                return new KlientB(db).GetKlientKeyAndValueItems();
            }
        }
        public ObservableCollection<KeyAndValue> PokojItems
        {
            get
            {
                var dostępnePokoje = db.Pokoj.Where(p =>
                    !db.Rezerwacja.Any(r =>
                        r.IdPokoju == p.IdPokoju &&
                        r.DataZameldowania < DataWymeldowania &&
                        r.DataWymeldowania > DataZameldowania))
                    .Select(p => new KeyAndValue
                    {
                        Key = p.IdPokoju,
                        Value = p.NrPokoju
                    }).ToList();

                if (item.IdRezerwacji != 0)  // tryb edycji - pobranie pokoju do comboboxa
                {
                    var currentPokoj = db.Rezerwacja.FirstOrDefault(r => r.IdRezerwacji == item.IdRezerwacji)?.IdPokoju;
                    if (currentPokoj.HasValue)
                    {
                        var pokoj = db.Pokoj.FirstOrDefault(p => p.IdPokoju == currentPokoj.Value);
                        if (pokoj != null)
                        {
                            dostępnePokoje.Insert(0, new KeyAndValue { Key = pokoj.IdPokoju, Value = pokoj.NrPokoju });
                        }
                    }
                }

                return new ObservableCollection<KeyAndValue>(dostępnePokoje);
            }
        }

        private Pokoj _selectedPokoj;
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
        public IQueryable<KeyAndValue> ZnizkaItems
        {
            get
            {
                return new ZnizkaB(db).GetZnizkaKeyAndValueItems();
            }
        }

        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IdKlienta):
                    return IdKlienta <= 0 ? "Wybierz klienta" : string.Empty;

                case nameof(IdPokoju):
                    return IdPokoju <= 0 ? "Wybierz pokój" : string.Empty;

                case nameof(DataZameldowania):
                    return DataZameldowania > DataWymeldowania ? "Data zameldowania nie może być późniejsza od daty wymeldowania" : string.Empty;

                case nameof(DataWymeldowania):
                    return DataWymeldowania < DataZameldowania ? "Data wymeldowania nie może poprzedzać daty zameldowania" : string.Empty;

                case nameof(DataRezerwacji):
                    return DataRezerwacji > DataZameldowania ? "Data rezerwacji nie może być późniejsza niż data zameldowania" : string.Empty;

                case nameof(LiczbaDoroslych):
                    if (string.IsNullOrWhiteSpace(LiczbaDoroslych) || !int.TryParse(LiczbaDoroslych, out int liczbaDoroslych) || liczbaDoroslych <= 0)
                    {
                        return "Wprowadź liczbę dorosłych";
                    }
                    return string.Empty;

                case nameof(LiczbaDzieci):  
                    return !Helper.StringValidator.ContainsOnlyNumbers(LiczbaDzieci) ? "Wprowadź liczbę dzieci" : string.Empty;
                
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region Methods
        public string GenerujNumerRezerwacji()
        {
            // pobranie ostatniej rezerwacji z BD do ustalenia następnego numeru
            var ostatniaRezerwacja = db.Rezerwacja
                                       .OrderByDescending(r => r.IdRezerwacji) 
                                       .Select(r => r.NrRezerwacji)
                                       .FirstOrDefault();

            string numerRezerwacji;

            if (ostatniaRezerwacja != null)
            {
                string rezerwacjaMiesiac = ostatniaRezerwacja.Substring(5, 2); // yyyy-MM
                string obecnyMiesiac = DateTime.Now.ToString("MM");

                if (rezerwacjaMiesiac == obecnyMiesiac)
                {
                    int pozycjaR = ostatniaRezerwacja.IndexOf('R');
                    if (pozycjaR != -1 && pozycjaR + 1 < ostatniaRezerwacja.Length)
                    {
                        string numer = ostatniaRezerwacja.Substring(pozycjaR + 1);

                        if (int.TryParse(numer, out int numerInt))
                        {
                            numerRezerwacji = (numerInt + 1).ToString();
                        }
                        else
                        {
                            numerRezerwacji = "1";
                        }
                    }
                    else
                    {
                        numerRezerwacji = "1";
                    }
                }
                else
                {
                    numerRezerwacji = "1";
                }
            }
            else
            {
                numerRezerwacji = "1";
            }
            return $"{DateTime.Now:yyyy-MM}-R{numerRezerwacji}";
        }

        private BaseCommand _obliczKwoteCommand;

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

        private void ObliczKwote()
        {
            if (SelectedPokoj == null || !int.TryParse(LiczbaDoroslych, out int liczbaDoroslych))
            {
                MessageBox.Show("Proszę wprowadzić poprawne dane we wszystkich polach.", "Błąd", MessageBoxButton.OK);
                return;
            }

            int liczbaDzieci = string.IsNullOrEmpty(LiczbaDzieci) ? 0 : int.Parse(LiczbaDzieci);
            int liczbaNocy = (DataWymeldowania - DataZameldowania).Days;

            if (liczbaNocy <= 0)
            {
                MessageBox.Show("Data wymeldowania musi być późniejsza niż data zameldowania.", "Błąd", MessageBoxButton.OK);
                return;
            }

            var cennik = new HotelEntities().Cennik
                .FirstOrDefault(c => c.IdKlasyPokoju == SelectedPokoj.IdKlasyPokoju && c.IdTypuPokoju == SelectedPokoj.IdTypuPokoju);

            if (cennik == null)
            {
                MessageBox.Show("Nie znaleziono odpowiedniego cennika.", "Błąd", MessageBoxButton.OK);
                return;
            }

            Kwota = ((liczbaDoroslych * cennik.CenaDorosly) + (liczbaDzieci * cennik.CenaDziecko) + (CzyZwierzeta ? cennik.CenaZwierzeta : 0)) * liczbaNocy;
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdRezerwacji == 0) // brak ID = insert
            {
                db.Rezerwacja.Add(item);
            }
            else // istnieje ID = update
            {
                var doEdycji = db.Rezerwacja.FirstOrDefault(f => f.IdRezerwacji == item.IdRezerwacji);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // automatyczne odświeżenie listy po edycji rekordu
            Messenger.Default.Send("RezerwacjaRefresh");
        }
        #endregion
    }
}
