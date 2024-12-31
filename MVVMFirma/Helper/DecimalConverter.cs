using System;
using System.Globalization;
using System.Windows.Data;

namespace MVVMFirma.Helper
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString(culture); // konwersja liczby na tekst wg regionu
            }
            return value?.ToString(); // jeśli wartosc nie jest decimal to zwraca tekst
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string tekst)
            {
                // zmiana przecinka na kropke
                tekst = tekst.Replace(',', '.');

                // próba zamiany tekstu na liczbe
                if (decimal.TryParse(tekst, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal wynik))
                {
                    return wynik; // zwraca decimal
                }
            }
            return 0m; // jesli błąd to zwraca 0 typu decimal
        }
    }

}