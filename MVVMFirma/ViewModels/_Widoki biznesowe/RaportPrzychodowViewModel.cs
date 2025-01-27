using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using MVVMFirma.Models;
using MVVMFirma.Models.Entities;
using System.Linq;
using MVVMFirma.Views;

namespace MVVMFirma.ViewModels
{
    public class RaportPrzychodowViewModel : WorkspaceViewModel
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Fields
        private DateTime _dataOd;
        private DateTime _dataDo;
        private decimal _przychodNetto;
        private decimal _przychodBrutto;
        private decimal _podatekVAT;
        private readonly PrzychodyB _przychodyB;
        #endregion

        #region Commands
        public ICommand ObliczPrzychodyCommand { get; }
        public ICommand SetCurrentMonthCommand { get; }
        public ICommand SetLastMonthCommand { get; }
        public ICommand SetLastYearCommand { get; }
        public ICommand SetCurrentYearCommand { get; }
        #endregion

        #region Properties
        public DateTime DataOd
        {
            get
            {
                return _dataOd;
            }

            set
            {
                _dataOd = value;
                OnPropertyChanged(() => DataOd);
            }
        }

        public DateTime DataDo
        {
            get
            {
                return _dataDo;
            }

            set
            {
                _dataDo = value;
                OnPropertyChanged(() => DataDo);
            }
        }

        public decimal PrzychodNetto
        {
            get
            {
                return _przychodNetto;
            }

            set
            {
                _przychodNetto = value;
                OnPropertyChanged(() => PrzychodNetto);
            }
        }

        public decimal PrzychodBrutto
        {
            get
            {
                return _przychodBrutto;
            }

            set
            {
                _przychodBrutto = value;
                OnPropertyChanged(() => PrzychodBrutto);
            }
        }

        public decimal PodatekVAT
        {
            get
            {
                return _podatekVAT;
            }

            set
            {
                _podatekVAT = value;
                OnPropertyChanged(() => PodatekVAT);
            }
        }
        #endregion

        #region Methods
        private void ObliczPrzychody()
        {
            try
            {
                if (DataOd > DataDo)
                {
                    ShowMessageBox("Data początkowa musi być wcześniej niż data końcowa.");
                    return;
                }

                var faktury = db.Faktura
                                .Where(f => f.DataSprzedazy >= DataOd && f.DataSprzedazy <= DataDo)
                                .ToList();

                PrzychodNetto = _przychodyB.ObliczNetto(faktury);
                PrzychodBrutto = _przychodyB.ObliczBrutto(faktury);
                PodatekVAT = _przychodyB.ObliczVAT(faktury);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Wystąpił błąd przy obliczaniu: {ex.Message}");
            }
        }
        private void SetCurrentMonth()
        {
            var now = DateTime.Today;
            DataOd = new DateTime(now.Year, now.Month, 1);
            DataDo = DataOd.AddMonths(1).AddDays(-1);
        }

        private void SetLastMonth()
        {
            var now = DateTime.Today;
            DataOd = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
            DataDo = new DateTime(now.Year, now.Month, 1).AddDays(-1);
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
        public RaportPrzychodowViewModel()
        {
            base.DisplayName = "Raport przychodów";

            db = new HotelEntities();

            _przychodyB = new PrzychodyB(db);

            ObliczPrzychodyCommand = new BaseCommand(ObliczPrzychody);
            SetLastYearCommand = new BaseCommand(SetLastYear);
            SetCurrentYearCommand = new BaseCommand(SetCurrentYear);
            SetLastMonthCommand = new BaseCommand(SetLastMonth);
            SetCurrentMonthCommand = new BaseCommand(SetCurrentMonth);

            // domyślnie zaznaczony jest bieżący miesiąc
            SetCurrentMonth();
        }
        #endregion
    }
}
