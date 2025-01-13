using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Helper
{
    public class MaxGuestsValidator
    {
        private readonly HotelEntities _db;

        public MaxGuestsValidator(HotelEntities db)
        {
            _db = db;
        }

        public string Validate(int idPokoju, int liczbaDoroslych, int liczbaDzieci)
        {
            var pokoj = _db.Pokoj.FirstOrDefault(p => p.IdPokoju == idPokoju);

            if (pokoj != null)
            {
                int maxLiczbaOsob = Convert.ToInt32(pokoj.TypPokoju.MaxLiczbaOsob);
                if ((liczbaDoroslych + liczbaDzieci) > maxLiczbaOsob)
                {
                    return $"za dużo gości ({liczbaDoroslych + liczbaDzieci}) na wybrany pokój (maksymalnie {maxLiczbaOsob})";
                }
            }

            return string.Empty;
        }
    }

}
