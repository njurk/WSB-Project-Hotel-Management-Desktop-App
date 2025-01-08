namespace MVVMFirma.Models.EntitiesForView
{
    public class KlientForAllView
    {
        public int IdKlienta { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Ulica { get; set; }
        public string NrDomu { get; set; }
        public string NrLokalu { get; set; }
        public string KodPocztowy { get; set; }
        public string Miasto { get; set; }
        public string Kraj { get; set; } // z klucza obcego
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string NIP { get; set; }
    }
}
