using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class UdogodnieniaKlasPokojuForAllView
    {
        public int IdRezerwacji {  get; set; }
        public int IdKlienta { get; set; } // klucz obcy
        public string IdKlientaImie { get; set; } // z klucza obcego
        public string IdKlientaNazwisko { get; set; } // z klucza obcego
        public int IdPracownika { get; set; } // klucz obcy
        public string IloscDoroslych {  get; set; }
        public string IloscDzieci { get; set; }
        public string IloscZwierzat { get; set; }
        public DateTime DataZameldowania { get; set; }
        public DateTime DataWymeldowania { get; set; }
        public int? IdPlatnosci { get; set; } // klucz obcy
        public DateTime DataRezerwacji { get; set; }
        public string Uwagi {  get; set; }
    }
}
