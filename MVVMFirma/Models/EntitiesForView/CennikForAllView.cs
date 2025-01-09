using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class CennikForAllView
    {
        public int IdCennika { get; set; }
        public string KlasaPokojuNazwa { get; set; } // z klucza obcego
        public string TypPokojuNazwa { get; set; } // z klucza obcego
        public decimal CenaDorosly { get; set; }
        public decimal CenaDziecko { get; set; }
        public decimal CenaZwierzeta { get; set; }
    }
}
