using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVVMFirma.Helper
{
    public class StringValidator
    {
        public static bool ContainsOnlyNumbers(string text)
        {
            return string.IsNullOrEmpty(text) || Regex.IsMatch(text, @"^\d+$");
        }

        public static bool ContainsOnlyLetters(string text)
        {
            return string.IsNullOrEmpty(text) || Regex.IsMatch(text, @"^[a-zA-Z]+$");
        }

        public static bool IsLengthInRange(string text, int minLength, int maxLength)
        {
            return !string.IsNullOrEmpty(text) && text.Length >= minLength && text.Length <= maxLength;
        }

        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\+?[0-9\s\-]+$");
        }

        public static bool IsValidPostalCode(string postalCode)
        {
            return Regex.IsMatch(postalCode, @"^\d{2}-\d{3}$");
        }


    }
}
