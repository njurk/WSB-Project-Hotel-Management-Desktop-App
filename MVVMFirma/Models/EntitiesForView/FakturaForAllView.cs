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
        public string KlientImie { get; set; } // z klucza obcego
        public string KlientNazwisko { get; set; } // z klucza obcego
        public string KlientNIP {  get; set; } // z klucza obcego
        public int IdRezerwacji { get; set; } // klucz obcy
        public string NrFaktury { get; set; }
        public string Opis {  get; set; }
        public DateTime DataWystawienia { get; set; }
        public DateTime DataSprzedazy { get; set; }
        public decimal KwotaNetto { get; set; }
        public decimal VAT { get; set; }
        public decimal KwotaBrutto { get; set; }
        public int? IdPlatnosci { get; set; } // klucz obcy
        public DateTime TerminPlatnosci { get; set; }
    }
}
