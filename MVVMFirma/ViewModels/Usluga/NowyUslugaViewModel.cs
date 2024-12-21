using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyUslugaViewModel : JedenViewModel<Usluga>
    {
        #region Constructor
        public NowyUslugaViewModel()
            :base("Usługa")
        {
            item = new Usluga();
        }
        #endregion

        #region Properties
        public int IdTypuUslugi
        {
            get
            {
                return item.IdTypuUslugi;
            }
            set
            {
                item.IdTypuUslugi = value;
                OnPropertyChanged(() => IdTypuUslugi);
            }
        }

        public DateTime DataRozpoczeciaUslugi
        {
            get
            {
                return item.DataRozpoczeciaUslugi;
            }
            set
            {
                item.DataRozpoczeciaUslugi = value;
                OnPropertyChanged(() => DataRozpoczeciaUslugi);
            }
        }

        public DateTime DataZakonczeniaUslugi
        {
            get
            {
                return item.DataZakonczeniaUslugi;
            }
            set
            {
                item.DataZakonczeniaUslugi = value;
                OnPropertyChanged(() => DataZakonczeniaUslugi);
            }
        }

        public int IdKlienta
        {
            get
            {
                return item.IdKlienta;
            }
            set
            {
                item.IdKlienta = value;
                OnPropertyChanged(() => IdKlienta);
            }
        }

        public int? IdPlatnosci
        {
            get
            {
                return item.IdPlatnosci;
            }
            set
            {
                item.IdPlatnosci = value;
                OnPropertyChanged(() => IdPlatnosci);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.Usluga.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
