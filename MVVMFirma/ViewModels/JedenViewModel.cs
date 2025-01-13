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
        #endregion

        #region Constructor
        public JedenViewModel(string displayName)
        {
            base.DisplayName = displayName;
            hotelEntities = new HotelEntities();
        }
        #endregion

        #region Helpers and Validation

        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                return ValidateProperty(columnName);
            }
        }

        // tą metodę (property) implementujemy w każdej klasie gdzie będzie walidacja
        protected virtual string ValidateProperty(string propertyName)
        {
            return string.Empty;
        }

        protected List<string> GetValidationErrors()
        {
            var allErrors = new List<string>();

            foreach (System.Reflection.PropertyInfo item in this.GetType().GetProperties())
            {
                string error = ValidateProperty(item.Name);
                if (!string.IsNullOrEmpty(error))
                {
                    allErrors.Add(error);
                }
            }
            return allErrors;
        }

        protected bool IsValid()
        {
            return GetValidationErrors().Count == 0; // true jeśli formularz nie ma błędów
        }

        private void ValidateAndSave()
        {
            try
            {
                var validationErrors = GetValidationErrors();

                if (validationErrors.Count > 0)
                {
                    string errorMessages = string.Join(Environment.NewLine + "- ", validationErrors);
                    MessageBox.Show("Proszę prawidłowo wypełnić pola!\n- " + errorMessages, "Błędy formularza", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    SaveAndClose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nieoczekiwany błąd:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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
