//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVVMFirma.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cennik
    {
        public int IdCennika { get; set; }
        public int IdKlasyPokoju { get; set; }
        public int IdTypuPokoju { get; set; }
        public decimal CenaDorosly { get; set; }
        public decimal CenaDziecko { get; set; }
        public decimal CenaZwierzeta { get; set; }
    
        public virtual KlasaPokoju KlasaPokoju { get; set; }
        public virtual TypPokoju TypPokoju { get; set; }
    }
}
