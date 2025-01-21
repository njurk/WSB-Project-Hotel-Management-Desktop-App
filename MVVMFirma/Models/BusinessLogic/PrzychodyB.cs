using MVVMFirma.Models.BusinessLogic;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.Models
{
    public class PrzychodyB : DatabaseClass
    {
        public PrzychodyB(HotelEntities db) 
            : base(db) { }

        public decimal ObliczNetto(IEnumerable<Faktura> faktury) =>
            faktury.Sum(f => f.KwotaNetto);

        public decimal ObliczBrutto(IEnumerable<Faktura> faktury) =>
            faktury.Sum(f => f.KwotaBrutto);

        public decimal ObliczVAT(IEnumerable<Faktura> faktury) =>
            ObliczBrutto(faktury) - ObliczNetto(faktury);
    }
}
