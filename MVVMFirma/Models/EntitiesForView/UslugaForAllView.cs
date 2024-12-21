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
        public string TypUslugiNazwa { get; set; }
        public DateTime DataRozpoczeciaUslugi { get; set; }
        public DateTime DataZakonczeniaUslugi { get; set; }
        public string KlientImie { get; set; }
        public string KlientNazwisko { get; set; }
    }
}
