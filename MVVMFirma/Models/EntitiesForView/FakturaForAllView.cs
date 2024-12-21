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
        public int IdKlienta { get; set; } // klucz obcy
        public string IdKlientaImie { get; set; } // z klucza obcego
        public string IdKlientaNazwisko { get; set; } // z klucza obcego
        public string NIP {  get; set; }
        public int IdRezerwacji { get; set; } // klucz obcy
        public string NrFaktury { get; set; }
        public string Opis {  get; set; }
        public DateTime DataWystawienia { get; set; }
        public DateTime DataSprzedazy { get; set; }
        public decimal KwotaNetto { get; set; }
        public decimal VAT { get; set; }
        public decimal KwotaBrutto { get; set; }
        public int IdPlatnosci { get; set; } // klucz obcy
        public DateTime TerminPlatnosci { get; set; }
    }
}
