using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using LiveCharts.Wpf;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class RaportOdwiedzinViewModel : WorkspaceViewModel
    {
        #region DB
        private readonly HotelEntities db;
        #endregion

        #region Fields
        private DateTime? _dataOd;
        private DateTime? _dataDo;
        private SeriesCollection _seriesCollection;
        private List<string> _labels;
        private int _liczbaGosci;
        private int _liczbaRezerwacji;
        #endregion

        #region Properties
        public DateTime? DataOd
        {
            get => _dataOd;
            set
            {
                _dataOd = value;
                OnPropertyChanged(() => DataOd);
            }
        }

        public DateTime? DataDo
        {
            get => _dataDo;
            set
            {
                _dataDo = value;
                OnPropertyChanged(() => DataDo);
            }
        }

        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged(() => SeriesCollection);
            }
        }

        public List<string> Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged(() => Labels);
            }
        }
        public int LiczbaGosci
        {
            get => _liczbaGosci;
            set
            {
                _liczbaGosci = value;
                OnPropertyChanged(() => LiczbaGosci);
            }
        }

        public int LiczbaRezerwacji
        {
            get => _liczbaRezerwacji;
            set
            {
                _liczbaRezerwacji = value;
                OnPropertyChanged(() => LiczbaRezerwacji);
            }
        }
        #endregion

        #region Commands
        public Func<double, string> YFormatter { get; set; }
        public BaseCommand ObliczCommand { get; }
        public BaseCommand SetCurrentMonthCommand { get; }
        public BaseCommand SetLastMonthCommand { get; }
        public BaseCommand SetLastYearCommand { get; }
        public BaseCommand SetCurrentYearCommand { get; }
        #endregion

        #region Methods
        private void Oblicz()
        {
            if (DataOd > DataDo)
            {
                ShowMessageBox("Data początkowa musi być wcześniej niż data końcowa.");
                return;
            }

            var dane = db.Rezerwacja
                         .Where(r => r.DataZameldowania < DataDo && r.DataWymeldowania > DataOd)
                         .AsEnumerable()
                         .Select(r => new
                         {
                             DataStart = r.DataZameldowania.Date,
                             DataEnd = r.DataWymeldowania.Date,
                             LiczbaRezerwacji = 1,
                             LiczbaGosci = ParseLiczba(r.LiczbaDoroslych) + ParseLiczba(r.LiczbaDzieci)
                         })
                         .ToList();

            // wszystkie dni danego zakresu ustawiamy do listy
            var wszystkieDaty = Enumerable.Range(0, (DataDo.Value - DataOd.Value).Days + 1)
                                          .Select(d => DataOd.Value.AddDays(d))
                                          .ToList();

            // ustawiamy listę dni jako etykiety wykresu
            Labels = wszystkieDaty.Select(d => d.ToString("dd.MM")).ToList();

            // dla każdego dnia z zakresu obliczamy ile rezerwacji aktualnie trwa - wykres nie
            // pokazuje dat dokonania rezerwacji tylko okresy trwania rezerwacji
            var liczbaRezerwacji = wszystkieDaty.Select(d =>
                dane.Where(x => x.DataStart <= d && x.DataEnd >= d)
                    .Sum(x => x.LiczbaRezerwacji)
            ).ToList();

            // tak samo obliczamy liczbę gości w hotelu dla każdego dnia z zakresu
            var liczbaGosci = wszystkieDaty.Select(d =>
                dane.Where(x => x.DataStart <= d && x.DataEnd >= d)
                    .Sum(x => x.LiczbaGosci)
            ).ToList();

            LiczbaRezerwacji = dane.Count();
            LiczbaGosci = dane.Sum(x => x.LiczbaGosci);

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Rezerwacje",
                    Values = new ChartValues<int>(liczbaRezerwacji)
                },
                new LineSeries
                {
                    Title = "Goście",
                    Values = new ChartValues<int>(liczbaGosci)
                }
            };
        }
        private int ParseLiczba(string liczba)
        {
            return int.TryParse(liczba, out int result) ? result : 0;
        }
        private void SetCurrentMonth()
        {
            var now = DateTime.Today;
            DataOd = new DateTime(now.Year, now.Month, 1);
            DataDo = DataOd.Value.AddMonths(1).AddDays(-1);
        }

        private void SetLastMonth()
        {
            var now = DateTime.Today;
            DataOd = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
            DataDo = DataOd.Value.AddMonths(1).AddDays(-1);
        }

        private void SetLastYear()
        {
            var now = DateTime.Today;
            DataOd = new DateTime(now.Year - 1, 1, 1);
            DataDo = new DateTime(now.Year - 1, 12, 31);
        }

        private void SetCurrentYear()
        {
            var now = DateTime.Today;
            DataOd = new DateTime(now.Year, 1, 1);
            DataDo = new DateTime(now.Year, 12, 31);
        }
        #endregion

        #region Constructor
        public RaportOdwiedzinViewModel()
        {
            DisplayName = "Raport odwiedzin";
            db = new HotelEntities();

            YFormatter = value => value.ToString();
            ObliczCommand = new BaseCommand(Oblicz);
            SetLastYearCommand = new BaseCommand(SetLastYear);
            SetCurrentYearCommand = new BaseCommand(SetCurrentYear);
            SetLastMonthCommand = new BaseCommand(SetLastMonth);
            SetCurrentMonthCommand = new BaseCommand(SetCurrentMonth);

            SetCurrentMonth();

            SeriesCollection = new SeriesCollection();
            Labels = new List<string>();
        }
        #endregion
    }
}
