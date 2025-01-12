using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public abstract class JedenViewModel<T> : WorkspaceViewModel, IDataErrorInfo
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
                    _SaveCommand = new BaseCommand(() => ValidateAndSave());
                return _SaveCommand;
            }
        }

        protected bool IsValid()
        {
            foreach (System.Reflection.PropertyInfo item in this.GetType().GetProperties())
            { 
                if (!string.IsNullOrEmpty(ValidateProperty(item.Name)))
                {
                    return false;
                }
            }
            return true;
        }

        public string Error { get; }

        // tą metodę (property) implementujemy w każdej klasie gdzie będzie walidacja
        public string this[string columnName] 
        { 
            get
            {
                return ValidateProperty(columnName);
            }
        }

        protected virtual string ValidateProperty(string propertyName)
        {
            return string.Empty;
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
        private void ValidateAndSave()
        {
            try
            {
                if (IsValid())
                {
                    SaveAndClose();
                }
                else
                {
                    MessageBox.Show("Nie można zapisać - błędnie wypełniony formularz!", "Błąd");
                }
            }
            catch
            {
                MessageBox.Show("Wystąpił błąd!", "Błąd");
            }
            
        }

        public abstract void Save();
        public void SaveAndClose()
        {
            Save();
            OnRequestClose();
            MessageBox.Show("Zmiany dokonane pomyślnie.", "Sukces");
        }
        #endregion
    }
}
