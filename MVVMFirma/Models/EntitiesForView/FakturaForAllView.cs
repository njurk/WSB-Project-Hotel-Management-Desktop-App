using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class FakturaForAllView
    {
        public int IdFaktury { get; set; }
        public string NrFaktury { get; set; }
        public int IdRezerwacji { get; set; } // klucz obcy
        public string KlientNIP { get; set; } // z klucza obcego
        public string KlientImie {  get; set; } // z klucza obcego
        public string KlientNazwisko {  get; set; } // z klucza obcego
        public DateTime DataWystawienia { get; set; }
        public DateTime DataSprzedazy { get; set; }
        public decimal KwotaNetto { get; set; }
        public string VAT { get; set; } // z klucza obcego
        public decimal KwotaBrutto { get; set; }
        public DateTime TerminPlatnosci { get; set; }
        public int IdPlatnosci { get; set; } // klucz obcy
        public string Opis { get; set; }
    }
}
