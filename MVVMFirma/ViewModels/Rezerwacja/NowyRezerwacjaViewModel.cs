using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                return item.DataZameldowania;
            }
            set
            {
                item.DataZameldowania = value;
                OnPropertyChanged(() => DataZameldowania);
            }
        }

        public DateTime DataWymeldowania
        {
            get
            {
                return item.DataWymeldowania;
            }
            set
            {
                item.DataWymeldowania = value;
                OnPropertyChanged(() => DataWymeldowania);
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

        public IQueryable<KeyAndValue> KlientItems
        {
            get
            {
                return new KlientB(db).GetKlientKeyAndValueItems();
            }
        }

        public IQueryable<KeyAndValue> PokojItems
        {
            get
            {
                return new PokojB(db).GetPokojKeyAndValueItems();
            }
        }

        protected override string ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IdKlienta):
                    return IdKlienta <= 0 ? "Proszę wybrać klienta" : string.Empty;

                case nameof(IdPokoju):
                    return IdPokoju <= 0 ? "Proszę wybrać pokój" : string.Empty;

                case nameof(DataZameldowania):
                    return DataZameldowania > DataWymeldowania ? "Data zameldowania nie może być późniejsza od daty wymeldowania" : string.Empty;

                case nameof(DataWymeldowania):
                    return DataWymeldowania < DataZameldowania ? "Data wymeldowania nie może poprzedzać daty zameldowania" : string.Empty;

                case nameof(DataRezerwacji):
                    return DataRezerwacji > DataZameldowania ? "Data rezerwacji nie może być późniejsza niż data zameldowania" : string.Empty;

                case nameof(LiczbaDoroslych):
                    if (string.IsNullOrWhiteSpace(LiczbaDoroslych) || !int.TryParse(LiczbaDoroslych, out int liczbaDoroslych) || liczbaDoroslych <= 0)
                    {
                        return "Proszę wprowadź liczbę dorosłych (co najmniej 1)";
                    }
                    return string.Empty;

                case nameof(LiczbaDzieci):  
                    return !Helper.StringValidator.ContainsOnlyNumbers(LiczbaDzieci) ? "Proszę wprowadź liczbę dzieci" : string.Empty;
                
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region Methods
        public string GenerujNumerRezerwacji()
        {
            var ostatniaRezerwacja = db.Rezerwacja
                                       .OrderByDescending(r => r.DataRezerwacji) 
                                       .ThenByDescending(r => r.IdRezerwacji)
                                       .Select(r => r.NrRezerwacji)
                                       .FirstOrDefault();

            string numerRezerwacji;

            if (ostatniaRezerwacja != null)
            {
                string rezerwacjaMiesiac = ostatniaRezerwacja.Substring(5, 2);
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


        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Rezerwacja.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
