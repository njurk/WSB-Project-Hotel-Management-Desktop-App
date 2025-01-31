using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using LiveCharts.Wpf;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVMFirma.ViewModels
{
    public class AnalizaPracownikowViewModel : WorkspaceViewModel
    {
        private string _imieNazwiskoPracownika;
        private int _idPracownika;
        private DateTime? _dataOd;
        private DateTime? _dataDo;
        private int _godzinOgolem;
        private int _nieobecnosci;
        private decimal _przewidywanaWyplata;
        private decimal _kosztPracownika;

        public int IdPracownika
        {
            get => _idPracownika;
            set
            {
                if (_idPracownika != value)
                {
                    _idPracownika = value;
                    OnPropertyChanged(() => IdPracownika);
                    using (var db = new HotelEntities())
                    {
                        var pracownik = db.Pracownik
                            .Where(p => p.IdPracownika == _idPracownika)
                            .Select(p => new { p.Imie, p.Nazwisko })
                            .FirstOrDefault();
                        if (pracownik != null)
                        {
                            ImieNazwiskoPracownika = $"{pracownik.Imie} {pracownik.Nazwisko}";
                        }
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
                OnPropertyChanged(() => ImieNazwiskoPracownika);
            }
        }

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

        public int GodzinOgolem
        {
            get => _godzinOgolem;
            set
            {
                _godzinOgolem = value;
                OnPropertyChanged(() => GodzinOgolem);
            }
        }

        public int Nieobecnosci
        {
            get => _nieobecnosci;
            set
            {
                _nieobecnosci = value;
                OnPropertyChanged(() => Nieobecnosci);
            }
        }

        public decimal PrzewidywanaWyplata
        {
            get => _przewidywanaWyplata;
            set
            {
                _przewidywanaWyplata = value;
                OnPropertyChanged(() => PrzewidywanaWyplata);
            }
        }

        public decimal KosztPracownika
        {
            get => _kosztPracownika;
            set
            {
                _kosztPracownika = value;
                OnPropertyChanged(() => KosztPracownika);
            }
        }

        public SeriesCollection SeriesCollection { get; set; }
        public ObservableCollection<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public BaseCommand SetPracownikCommand { get; }
        public BaseCommand SetLastMonthCommand { get; }
        public BaseCommand SetCurrentMonthCommand { get; }
        public BaseCommand ObliczCommand { get; }

        public AnalizaPracownikowViewModel()
        {
            base.DisplayName = "Analiza pracowników";

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Godziny pracy",
                    Values = new ChartValues<int>()
                }
            };

            Labels = new ObservableCollection<string>();
            YFormatter = value => value.ToString();

            SetPracownikCommand = new BaseCommand(SetPracownik);
            SetLastMonthCommand = new BaseCommand(SetLastMonth);
            SetCurrentMonthCommand = new BaseCommand(SetCurrentMonth);
            ObliczCommand = new BaseCommand(ObliczWszystko);

            Messenger.Default.Register<int>(this, idPracownika => IdPracownika = idPracownika);
            SetCurrentMonth();
        }

        private void SetPracownik()
        {
            OpenPracownicyModalne();
        }

        private void OpenPracownicyModalne()
        {
            new PracownicyModalneView().ShowDialog();
        }

        private void SetLastMonth()
        {
            var today = DateTime.Now;
            var firstDayLastMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
            var lastDayLastMonth = firstDayLastMonth.AddMonths(1).AddDays(-1);
            DataOd = firstDayLastMonth;
            DataDo = lastDayLastMonth;
        }

        private void SetCurrentMonth()
        {
            var today = DateTime.Now;
            var firstDayCurrentMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayCurrentMonth = firstDayCurrentMonth.AddMonths(1).AddDays(-1);
            DataOd = firstDayCurrentMonth;
            DataDo = lastDayCurrentMonth;
        }

        private void ObliczWszystko()
        {
            // sprawdzenie czy jest wybrany pracownik oraz daty od i do
            var errors = Validate();
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show($"Popraw błędy:\n{errors}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // zaczynamy na nowym, czystym wykresie
            SeriesCollection.Clear();
            Labels.Clear();

            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Godziny pracy",
                Values = new ChartValues<int>()
            });

            // zakres dni z pól ustawiany jest na oś x wykresu
            var days = (DataDo.Value - DataOd.Value).Days + 1;
            for (var i = 0; i < days; i++)
            {
                var date = DataOd.Value.AddDays(i);
                Labels.Add(date.ToString("dd.MM"));
            }

            using (var db = new HotelEntities())
            {
                // pobranie godzin pracy praocwnika z danego okresu czasu
                var godzinyPracy = db.Obecnosc
                    .Where(o => o.IdPracownika == IdPracownika && o.Data >= DataOd && o.Data <= DataDo)
                    .Select(o => new { o.Data, o.GodzinaRozpoczecia, o.GodzinaZakonczenia })
                    .ToList();

                var workHours = new List<int>(new int[days]);
                // lista z godzinami i indeksem względem początku wykresu
                foreach (var godzina in godzinyPracy)
                {
                    var hoursWorked = (godzina.GodzinaZakonczenia - godzina.GodzinaRozpoczecia)?.TotalHours ?? 0;
                    var dateIndex = (godzina.Data - DataOd.Value).Days;
                    workHours[dateIndex] = (int)hoursWorked;
                }

                // przypisanie danych z powyżej utworzonej listy na wykres
                SeriesCollection[0].Values.AddRange(workHours.Cast<object>());

                GodzinOgolem = workHours.Sum();
                Nieobecnosci = db.Obecnosc
                    .Count(record => record.IdPracownika == IdPracownika &&
                                     record.Data >= DataOd &&
                                     record.Data <= DataDo &&
                                     record.CzyUsprawiedliwiony);

                var stanowiskoStawka = db.Pracownik
                    .Where(p => p.IdPracownika == IdPracownika)
                    .Select(p => p.Stanowisko.StawkaGodzinowa)
                    .FirstOrDefault();

                PrzewidywanaWyplata = GodzinOgolem * stanowiskoStawka;
                KosztPracownika = PrzewidywanaWyplata + (PrzewidywanaWyplata * 0.23m);
            }
        }

        private string Validate()
        {
            var errors = string.Empty;

            if (IdPracownika == 0)
                errors += "- Wybierz pracownika.\n";

            if (!DataOd.HasValue)
                errors += "- Wybierz datę początkową.\n";

            if (!DataDo.HasValue)
                errors += "- Wybierz datę końcową.\n";

            if (DataOd.HasValue && DataDo.HasValue && DataOd > DataDo)
                errors += "- Data początkowa nie może być późniejsza niż data końcowa.\n";

            return errors;
        }
    }
}
