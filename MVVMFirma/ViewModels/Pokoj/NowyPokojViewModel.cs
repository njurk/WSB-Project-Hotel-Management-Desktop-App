using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyPokojViewModel : JedenViewModel<Pokoj>
    {
        #region Constructor
        public NowyPokojViewModel()
            :base("Pokój")
        {
            item = new Pokoj();
        }
        #endregion

        #region Properties
        public string NrPokoju
        {
            get
            {
                return item.NrPokoju;
            }
            set
            {
                item.NrPokoju = value;
                OnPropertyChanged(() => NrPokoju);
            }
        }

        public int IdTypuPokoju
        {
            get
            {
                return item.IdTypuPokoju;
            }
            set
            {
                item.IdTypuPokoju = value;
                OnPropertyChanged(() => IdTypuPokoju);
            }
        }

        public int IdKlasyPokoju
        {
            get
            {
                return item.IdKlasyPokoju;
            }
            set
            {
                item.IdKlasyPokoju = value;
                OnPropertyChanged(() => IdKlasyPokoju);
            }
        }

        public int IdStatusuPokoju
        {
            get
            {
                return item.IdStatusuPokoju;
            }
            set
            {
                item.IdStatusuPokoju = value;
                OnPropertyChanged(() => IdStatusuPokoju);
            }
        }

        public int IdPietra
        {
            get
            {
                return item.IdPietra;
            }
            set
            {
                item.IdPietra = value;
                OnPropertyChanged(() => IdPietra);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Pokoj.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
