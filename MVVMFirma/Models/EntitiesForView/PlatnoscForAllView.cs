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
        public string KlientImie { get; set; } // z klucza obcego
        public string KlientNazwisko { get; set; } // z klucza obcego
        public string SposobPlatnosciNazwa { get; set; } // z klucza obcego
        public string StatusPlatnosciNazwa { get; set; } // z klucza obcego
        public DateTime? DataPlatnosci { get; set; }
        public decimal Kwota {  get; set; }
    }
}
