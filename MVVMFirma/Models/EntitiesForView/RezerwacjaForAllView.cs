using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class RezerwacjaForAllView
    {
        public int IdRezerwacji {  get; set; }
        public string KlientImie { get; set; } // z klucza obcego
        public string KlientNazwisko { get; set; } // z klucza obcego
        public string NrPokoju { get; set; } // klucz obcy
        public int IdPracownika { get; set; } // klucz obcy
        public string IloscDoroslych {  get; set; }
        public string IloscDzieci { get; set; }
        public string IloscZwierzat { get; set; }
        public DateTime DataZameldowania { get; set; }
        public DateTime DataWymeldowania { get; set; }
        public DateTime DataRezerwacji { get; set; }
        public string Uwagi {  get; set; }
    }
}
