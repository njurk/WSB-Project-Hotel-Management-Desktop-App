using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NowyObecnoscViewModel : JedenViewModel<Obecnosc>
    {
        #region DB
        private readonly HotelEntities db;
        #endregion

        #region Fields
        private string _imieNazwiskoPracownika;
        private BaseCommand _openPracownicyModalne;
        #endregion

        #region Properties
        public int IdPracownika
        {
            get
            {
                return item.IdPracownika;
            }
            set
            {
                if (item.IdPracownika != value)
                {
                    item.IdPracownika = value;
                    OnPropertyChanged(() => IdPracownika);

                    var pracownik = db.Pracownik.FirstOrDefault(p => p.IdPracownika == value);
                    if (pracownik != null)
                    {
                        ImieNazwiskoPracownika = pracownik.Imie + " " + pracownik.Nazwisko;
                        OnPropertyChanged(() => ImieNazwiskoPracownika);
                    }
                }
            }
        }
        public string ImieNazwiskoPracownika
        {
            get => _imieNazwiskoPracownika;
            set
            {
                _imieNazwiskoPracownika = value;
                OnPropertyChanged(() => _imieNazwiskoPracownika);
            }
        }

        public DateTime Data
        {
            get
            {
                return item.Data;
            }
            set
            {
                item.Data = value;
                OnPropertyChanged(() => Data);
            }
        }

        public bool CzyObecny
        {
            get
            {
                return item.CzyObecny;
            }
            set
            {
                item.CzyObecny = value;
                OnPropertyChanged(() => CzyObecny);
                // przy każdej zmianie stanu pola czyszczą się aby uniknąć wprowadzenia sprzecznych danych
                if (!value)
                {
                    GodzinaRozpoczecia = null;
                    GodzinaZakonczenia = null;
                    CzyUsprawiedliwiony = false;
                } 
                else
                {
                    CzyUsprawiedliwiony = false;
                }
                // trigger do zmian stanu pól zależnie od obecności
                OnPropertyChanged(() => IsGodzinyEnabled);
                OnPropertyChanged(() => IsUsprawiedliwionyEnabled);
                // odświeżenie walidacji godzin
                OnPropertyChanged(() => GodzinaRozpoczecia);
                OnPropertyChanged(() => GodzinaZakonczenia);
            }
        }

        public TimeSpan? GodzinaRozpoczecia
        {
            get
            {
                return item.GodzinaRozpoczecia;
            }
            set
            {
                item.GodzinaRozpoczecia = value;
                OnPropertyChanged(() => GodzinaRozpoczecia);
            }
        }

        public TimeSpan? GodzinaZakonczenia
        {
            get
            {
                return item.GodzinaZakonczenia;
            }
            set
            {
                item.GodzinaZakonczenia = value;
                OnPropertyChanged(() => GodzinaZakonczenia);
            }
        }

        public bool CzyUsprawiedliwiony
        {
            get
            {
                return item.CzyUsprawiedliwiony;
            }
            set
            {
                item.CzyUsprawiedliwiony = value;
                OnPropertyChanged(() => CzyUsprawiedliwiony);
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

        public bool IsGodzinyEnabled => CzyObecny; // aktywne tylko kiedy czyobecny=true
        public bool IsUsprawiedliwionyEnabled => !CzyObecny; // aktywne tylko kiedy czyobecny=false
        #endregion

        #region Items
        public IEnumerable<KeyAndValue> PracownikItems
        {
            get
            {
                return new PracownikB(db).GetPracownikKeyAndValueItems();
            }
        }
        #endregion

        #region Commands
        public BaseCommand OpenPracownicyModalneCommand
        {
            get
            {
                if (_openPracownicyModalne == null)
                {
                    _openPracownicyModalne = new BaseCommand(OpenPracownicyModalne);
                }
                return _openPracownicyModalne;
            }
        }

        private void OpenPracownicyModalne()
        {
            var pracownicyModalne = new PracownicyModalneView();
            pracownicyModalne.ShowDialog();
        }
        #endregion

        #region Methods
        public override void Save()
        {
            if (item.IdObecnosci == 0)
            {
                db.Obecnosc.Add(item);
            }
            else
            {
                var doEdycji = db.Obecnosc.FirstOrDefault(o => o.IdObecnosci == item.IdObecnosci);
                if (doEdycji != null)
                {
                    db.Entry(doEdycji).CurrentValues.SetValues(item);
                }
            }
            db.SaveChanges();
            Messenger.Default.Send("ObecnoscRefresh");
        }
        #endregion

        #region Validation
        protected override string ValidateProperty(string propertyName)
        {
            if (CzyObecny)
            {
                switch (propertyName)
                {
                    case nameof(GodzinaRozpoczecia):
                        return GodzinaRozpoczecia == null || GodzinaRozpoczecia >= GodzinaZakonczenia
                            ? "wprowadź poprawną godzinę rozpoczęcia"
                            : string.Empty;

                    case nameof(GodzinaZakonczenia):
                        return GodzinaZakonczenia == null || GodzinaZakonczenia <= GodzinaRozpoczecia
                            ? "wprowadź poprawną godzinę zakończenia"
                            : string.Empty;

                    case nameof(CzyUsprawiedliwiony):
                        return string.Empty;
                }
            }

            if (!CzyObecny)
            {
                switch (propertyName)
                {
                    case nameof(GodzinaRozpoczecia):
                    case nameof(GodzinaZakonczenia):
                        return string.Empty;

                    case nameof(CzyUsprawiedliwiony):
                        return string.Empty;
                }
            }

            switch (propertyName)
            {
                case nameof(ImieNazwiskoPracownika):
                    return string.IsNullOrEmpty(ImieNazwiskoPracownika) ? "wybierz pracownika" : string.Empty;

                case nameof(Data):
                    return Data == default || Data > DateTime.Now ? "wprowadź poprawną datę" : string.Empty;

                default:
                    return string.Empty;
            }
        }


        #endregion

        #region Constructors
        public NowyObecnoscViewModel()
            : base("Obecność")
        {
            db = new HotelEntities();
            item = new Obecnosc();
            Data = DateTime.Now;
            CzyObecny = true;

            Messenger.Default.Register<int>(this, idPracownika => IdPracownika = idPracownika);
        }

        public NowyObecnoscViewModel(int itemId)
            : base("Edycja Obecności")
        {
            db = new HotelEntities();
            item = db.Obecnosc.FirstOrDefault(o => o.IdObecnosci == itemId);

            if (item != null)
            {
                IdPracownika = item.IdPracownika;
                ImieNazwiskoPracownika = item.Pracownik.Imie + " " + item.Pracownik.Nazwisko;
                Data = item.Data;
                CzyObecny = item.CzyObecny;
                GodzinaRozpoczecia = item.GodzinaRozpoczecia;
                GodzinaZakonczenia = item.GodzinaZakonczenia;
                CzyUsprawiedliwiony = item.CzyUsprawiedliwiony;
                Uwagi = item.Uwagi;
            }
            Messenger.Default.Register<int>(this, idPracownika => IdPracownika = idPracownika);
        }
        #endregion
    }
}
