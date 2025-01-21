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
        #region Fields
        private DateTime _dataOd;
        private DateTime _dataDo;
        private decimal _przychodNetto;
        private decimal _przychodBrutto;
        private decimal _podatekVAT;
        private readonly PrzychodyB _przychodyB;
        #endregion

        #region DB
        HotelEntities db;
        #endregion

        public RaportPrzychodowViewModel()
        {
            base.DisplayName = "Raport przychodów";

            db = new HotelEntities();
            
            _przychodyB = new PrzychodyB(db);
            DataOd = DateTime.Today;
            DataDo = DateTime.Today.AddDays(1);

            ObliczPrzychodyCommand = new BaseCommand(ObliczPrzychody);
        }

        public DateTime DataOd
        {
            get => _dataOd;
            set
            {
                _dataOd = value;
                OnPropertyChanged(() => DataOd);
            }
        }

        public DateTime DataDo
        {
            get => _dataDo;
            set
            {
                _dataDo = value;
                OnPropertyChanged(() => DataDo);
            }
        }

        public decimal PrzychodNetto
        {
            get => _przychodNetto;
            set
            {
                _przychodNetto = value;
                OnPropertyChanged(() => PrzychodNetto);
            }
        }

        public decimal PrzychodBrutto
        {
            get => _przychodBrutto;
            set
            {
                _przychodBrutto = value;
                OnPropertyChanged(() => PrzychodBrutto);
            }
        }

        public decimal PodatekVAT
        {
            get => _podatekVAT;
            set
            {
                _podatekVAT = value;
                OnPropertyChanged(() => PodatekVAT);
            }
        }

        public ICommand ObliczPrzychodyCommand { get; }

        private void ObliczPrzychody()
        {
            try
            {
                if (DataOd > DataDo)
                {
                    return; // obsłużyć błąd
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
                ShowMessageBox($"Wystąpił błąd podczas obliczania przychodów: {ex.Message}");
            }
        }
    }
}
