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
        public int IdRezerwacji { get; set; } // klucz obcy
        public string TypUslugiNazwa { get; set; } // z klucza obcego
        public DateTime DataRozpoczeciaUslugi { get; set; }
        public DateTime DataZakonczeniaUslugi { get; set; }
        public decimal Kwota { get; set; }
    }
}
