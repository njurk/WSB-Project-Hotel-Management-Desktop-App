
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
    
public partial class Klient
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Klient()
    {

        this.Faktura = new HashSet<Faktura>();

        this.Platnosc = new HashSet<Platnosc>();

        this.Rezerwacja = new HashSet<Rezerwacja>();

        this.Usluga = new HashSet<Usluga>();

    }


    public int IdKlienta { get; set; }

    public string Imie { get; set; }

    public string Nazwisko { get; set; }

    public string Ulica { get; set; }

    public string NrDomu { get; set; }

    public string NrLokalu { get; set; }

    public string KodPocztowy { get; set; }

    public string Miasto { get; set; }

    public string Kraj { get; set; }

    public string Email { get; set; }

    public string Telefon { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Faktura> Faktura { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Platnosc> Platnosc { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Rezerwacja> Rezerwacja { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Usluga> Usluga { get; set; }

}

}
