
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
    
public partial class UdogodnieniaKlasPokoju
{

    public int IdPolaczenia { get; set; }

    public int IdKlasyPokoju { get; set; }

    public int IdUdogodnienia { get; set; }



    public virtual KlasaPokoju KlasaPokoju { get; set; }

    public virtual Udogodnienie Udogodnienie { get; set; }

}

}
