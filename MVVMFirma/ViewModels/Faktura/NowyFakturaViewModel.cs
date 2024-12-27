﻿using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyFakturaViewModel : JedenViewModel<Faktura>
    {
        #region DB
        HotelEntities db;
        #endregion

        #region Constructor
        public NowyFakturaViewModel()
            : base("Faktura")
        {
            item = new Faktura();
            DataWystawienia = DateTime.Now;
            DataSprzedazy = DateTime.Now;
            TerminPlatnosci = DateTime.Now.AddDays(14);
            db = new HotelEntities();
        }
        #endregion

        #region Properties
        public string NrFaktury
        {
            get
            {
                return item.NrFaktury;
            }
            set
            {
                item.NrFaktury = value;
                OnPropertyChanged(() => NrFaktury);
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

        public DateTime DataWystawienia
        {
            get
            {
                return item.DataWystawienia;
            }
            set
            {
                item.DataWystawienia = value;
                OnPropertyChanged(() => DataWystawienia);
            }
        }

        public DateTime DataSprzedazy
        {
            get
            {
                return item.DataSprzedazy;
            }
            set
            {
                item.DataSprzedazy = value;
                OnPropertyChanged(() => DataSprzedazy);
            }
        }
        public decimal KwotaBrutto
        {
            get
            {
                return item.KwotaBrutto;
            }
            set
            {
                item.KwotaBrutto = value;
                OnPropertyChanged(() => KwotaBrutto);
            }
        }

        public int IdVat
        {
            get
            {
                return item.IdVat;
            }
            set
            {
                item.IdVat = value;
                OnPropertyChanged(() => IdVat);
            }
        }

        public decimal KwotaNetto
        {
            get
            {
                return item.KwotaNetto;
            }
            set
            {
                item.KwotaNetto = value;
                OnPropertyChanged(() => KwotaNetto);
            }
        }
       
        public DateTime TerminPlatnosci
        {
            get
            {
                return item.TerminPlatnosci;
            }
            set
            {
                item.TerminPlatnosci = value;
                OnPropertyChanged(() => TerminPlatnosci);
            }
        }
        public string Opis
        {
            get
            {
                return item.Opis;
            }
            set
            {
                item.Opis = value;
                OnPropertyChanged(() => Opis);
            }
        }

        public IQueryable<KeyAndValue> PlatnoscItems
        {
            get
            {
                return new PlatnoscB(db).GetPlatnoscKeyAndValueItems();
            }
        }

        public IQueryable<KeyAndValue> VATItems
        {
            get
            {
                return new VATB(db).GetVATKeyAndValueItems();
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Faktura.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
