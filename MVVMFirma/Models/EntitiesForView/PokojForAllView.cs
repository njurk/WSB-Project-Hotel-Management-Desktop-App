namespace MVVMFirma.Models.EntitiesForView
{
    public class PokojForAllView
    {
        public int IdPokoju { get; set; }
        public string NrPokoju { get; set; }
        public string TypPokojuNazwa { get; set; } // z klucza obcego
        public string KlasaPokojuNazwa { get; set; } // z klucza obcego
        public bool CzyZajety { get; set; }

    }
}
