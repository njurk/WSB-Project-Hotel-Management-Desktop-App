using System;

public class ObecnoscForAllView
{
    public int IdObecnosci { get; set; }
    public string PracownikImie { get; set; } // z klucza obcego
    public string PracownikNazwisko { get; set; }// z klucza obcego
    public DateTime Data { get; set; }
    public bool CzyObecny { get; set; }
    public TimeSpan? GodzinaRozpoczecia { get; set; }
    public TimeSpan? GodzinaZakonczenia { get; set; }
    public bool CzyUsprawiedliwiony { get; set; }
    public string Uwagi { get; set; }
}
