using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            // dla każdego propertisa z danej instancji klasy wywołuje validateproperty i zbiera błędy
            foreach (System.Reflection.PropertyInfo item in this.GetType().GetProperties())
            {
                string error = ValidateProperty(item.Name);
                if (!string.IsNullOrEmpty(error))
                {
                    allErrors.Add(error);
                }
            }

            var validateDuplicateMethod = this.GetType().GetMethod("ValidateDuplicate");

            if (validateDuplicateMethod != null)
            {
                // wywołanie ValidateDuplicate jeśli dostępna
                var result = validateDuplicateMethod.Invoke(this, null) as string;
                if (!string.IsNullOrEmpty(result))
                {
                    allErrors.Add(result);
                }
            }

            return allErrors;
        }


        private void ValidateAndSave()
        {
            try
            {
                var validationErrors = GetValidationErrors();

                if (validationErrors.Count > 0)
                {
                    string errorMessages = string.Join(Environment.NewLine + "- ", validationErrors);
                    MessageBox.Show("Proszę poprawić formularz\n- " + errorMessages, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    SaveAndCloseSuccess();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nieoczekiwany błąd:\n" + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public abstract void Save();
        public void SaveAndCloseSuccess()
        {
            Save();
            OnRequestClose();
            MessageBox.Show("Zmiany dokonane pomyślnie", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
