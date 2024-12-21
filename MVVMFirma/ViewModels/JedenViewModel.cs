using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public abstract class JedenViewModel<T> : WorkspaceViewModel
    {
        #region DB
        protected HotelEntities hotelEntities;
        #endregion

        #region Item
        protected T item;
        #endregion

        #region Command
        private BaseCommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                    _SaveCommand = new BaseCommand(() => SaveAndClose());
                return _SaveCommand;
            }
        }
        #endregion

        #region Constructor
        public JedenViewModel(string displayName)
        {
            base.DisplayName = displayName;
            hotelEntities = new HotelEntities();
        }
        #endregion

        #region Helpers
        public abstract void Save();
        public void SaveAndClose()
        {
            Save();
            OnRequestClose();
        }
        #endregion
    }
}
