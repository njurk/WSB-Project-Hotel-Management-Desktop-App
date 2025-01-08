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
