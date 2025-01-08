using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyPlatnoscViewModel : JedenViewModel<Platnosc>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyPlatnoscViewModel()
            : base("Płatność")
        {
            db = new HotelEntities();
            item = new Platnosc();

            DataPlatnosci = DateTime.Now;
            NrPlatnosci = GenerujNumerPlatnosci();
        }

        public NowyPlatnoscViewModel(int itemId)
            : base("Edycja płatności")
        {
            db = new HotelEntities();
            item = db.Platnosc.FirstOrDefault(p => p.IdPlatnosci == itemId);

            if (item != null)
            {
                NrPlatnosci = item.NrPlatnosci;
                IdRezerwacji = item.IdRezerwacji;
                IdSposobuPlatnosci = item.IdSposobuPlatnosci;
                IdStatusuPlatnosci = item.IdStatusuPlatnosci;
                DataPlatnosci = item.DataPlatnosci;
                Kwota = item.Kwota;
            }
        }
        #endregion

        #region Properties
        public string NrPlatnosci
        {
            get
            {
                return item.NrPlatnosci;
            }
            set
            {
                item.NrPlatnosci = value;
                OnPropertyChanged(() => NrPlatnosci);
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
                item.IdRezerwacji = value;
                OnPropertyChanged(() => IdRezerwacji);
                SelectedRezerwacja = db.Rezerwacja.FirstOrDefault(r => r.IdRezerwacji == value);
            }
        }

        public int IdSposobuPlatnosci
        {
            get
            {
                return item.IdSposobuPlatnosci;
            }
            set
            {
                item.IdSposobuPlatnosci = value;
                OnPropertyChanged(() => IdSposobuPlatnosci);
            }
        }

        public int IdStatusuPlatnosci
        {
            get
            {
                return item.IdStatusuPlatnosci;
            }
            set
            {
                item.IdStatusuPlatnosci = value;
                OnPropertyChanged(() => IdStatusuPlatnosci);
            }
        }

        public DateTime DataPlatnosci
        {
            get
            {
                return item.DataPlatnosci;
            }
            set
            {
                item.DataPlatnosci = value;
                OnPropertyChanged(() => DataPlatnosci);
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

        private decimal _doZaplaty;
        public decimal DoZaplaty
        {
            get { return _doZaplaty; }
            set
            {
                if (_doZaplaty != value)
                {
                    _doZaplaty = value;
                    OnPropertyChanged(() => DoZaplaty);
                }
            }
        }


        public IQueryable<KeyAndValue> RezerwacjaItems
        {
            get
            {
                return new RezerwacjaB(db).GetRezerwacjaKeyAndValueItems();
            }
        }

        public IQueryable<KeyAndValue> SposobPlatnosciItems
        {
            get
            {
                return new SposobPlatnosciB(db).GetSposobPlatnosciKeyAndValueItems();
            }
        }

        public IQueryable<KeyAndValue> StatusPlatnosciItems
        {
            get
            {
                return new StatusPlatnosciB(db).GetStatusPlatnosciKeyAndValueItems();
            }
        }

        // ustawienie kwoty płatności na podstawie wybranej rezerwacji w combobox
        private Rezerwacja _selectedRezerwacja;
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
                        var rezerwacja = db.Rezerwacja.FirstOrDefault(r => r.IdRezerwacji == _selectedRezerwacja.IdRezerwacji);
                        if (rezerwacja != null)
                        {
                            Kwota = rezerwacja.Kwota;
                            DoZaplaty = Kwota - sumaPlatnosci(rezerwacja.IdRezerwacji);

                            OnPropertyChanged(() => Kwota);
                            OnPropertyChanged(() => DoZaplaty);
                        }
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
        #endregion

        #region Methods
        private string GenerujNumerPlatnosci()
        {
            // pobranie ostatniej platnosci z BD do ustalenia następnego numeru
            var ostatniaPlatnosc = db.Platnosc
                                     .OrderByDescending(f => f.IdPlatnosci)
                                     .Select(f => f.NrPlatnosci)
                                     .FirstOrDefault();

            string numerPlatnosci;

            if (ostatniaPlatnosc != null)
            {
                string platnoscMiesiac = ostatniaPlatnosc.Substring(5, 2); // yyyy-MM
                string obecnyMiesiac = DateTime.Now.ToString("MM");

                if (platnoscMiesiac == obecnyMiesiac)
                {
                    int pozycjaP = ostatniaPlatnosc.IndexOf('P');
                    if (pozycjaP != -1 && pozycjaP + 1 < ostatniaPlatnosc.Length)
                    {
                        string numer = ostatniaPlatnosc.Substring(pozycjaP + 1);

                        if (int.TryParse(numer, out int numerInt))
                        {
                            numerPlatnosci = (numerInt + 1).ToString();
                        }
                        else
                        {
                            numerPlatnosci = "1";
                        }
                    }
                    else
                    {
                        numerPlatnosci = "1";
                    }
                }
                else
                {
                    numerPlatnosci = "1";
                }
            }
            else
            {
                numerPlatnosci = "1";
            }
            return $"{DateTime.Now:yyyy-MM}-P{numerPlatnosci}";
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (item.IdPlatnosci == 0) // brak ID = insert
            {
                db.Platnosc.Add(item);
            }
            else // istnieje ID = update
            {
                var doEdycji = db.Platnosc.FirstOrDefault(f => f.IdPlatnosci == item.IdPlatnosci);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }

            db.SaveChanges();
            // automatyczne odświeżenie listy po edycji rekordu
            Messenger.Default.Send("PlatnoscRefresh");
        }
        #endregion

    }
}
