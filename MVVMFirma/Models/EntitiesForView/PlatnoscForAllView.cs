using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class PlatnoscForAllView
    {
        public int IdPlatnosci {  get; set; }
        public int IdKlienta { get; set; } // klucz obcy
        public string IdKlientaImie { get; set; } // z klucza obcego
        public string IdKlientaNazwisko { get; set; } // z klucza obcego
        public string IdSposobuPlatnosciNazwa { get; set; } // z klucza obcego
        public string IdStatusuPlatnosciNazwa { get; set; } // z klucza obcego
        public DateTime? DataPlatnosci { get; set; }
        public decimal Kwota {  get; set; }
    }
}
