using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class UslugaForAllView
    {
        public int IdUslugi { get; set; }
        public string IdTypuUslugiNazwa { get; set; }
        public DateTime DataRozpoczeciaUslugi { get; set; }
        public DateTime DataZakonczeniaUslugi { get; set; }
        public string IdKlientaImie { get; set; }
        public string IdKlientaNazwisko { get; set; }
        public int? IdPlatnosci { get; set; }
    }
}
