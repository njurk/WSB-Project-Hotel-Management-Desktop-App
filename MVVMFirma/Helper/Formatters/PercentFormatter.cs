using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMFirma.Helper
{
    public class PercentFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.Equals(0m))
                return string.Empty;
            return string.Format("{0}%", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
