using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
            return string.IsNullOrEmpty(text) || Regex.IsMatch(text, @"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ]+$");
        }
        public static bool ContainsOnlyLettersWithSpaces(string text)
        {
            // dozwolone małe i duże litery oraz spacje
            return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text) || !Regex.IsMatch(text, @"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s]+$");
        }

        public static bool IsLengthInRange(string text, int minLength, int maxLength)
        {
            return !string.IsNullOrEmpty(text) && text.Length >= minLength && text.Length <= maxLength;
        }

        public static bool IsValidStreet(string street)
        {
            // dozwolone duże i małe litery, spacje, - i .
            return string.IsNullOrEmpty(street) || string.IsNullOrWhiteSpace(street) || !Regex.IsMatch(street, @"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ0-9\s.\-]+$");
        }

        public static bool IsValidHouseNumber(string houseNumber)
        {
            // liczby dodatnie + opcjonalnie duże i małe litery, bez spacji
            return string.IsNullOrEmpty(houseNumber) || string.IsNullOrWhiteSpace(houseNumber) || !Regex.IsMatch(houseNumber, @"^[1-9]\d*[a-zA-Z]*$");
        }

        public static bool IsValidPostalCode(string postalCode)
        {
            return string.IsNullOrEmpty(postalCode) || string.IsNullOrWhiteSpace(postalCode) || !Regex.IsMatch(postalCode, @"^\d{2}-\d{3}$");
        }

        public static bool IsValidCity(string city)
        {
            // dozwolone są litery, spacje i myślniki
            return string.IsNullOrEmpty(city) || string.IsNullOrWhiteSpace(city) || !Regex.IsMatch(city, @"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ\s\-]+$");
        }

        public static bool IsValidDateOfBirth(DateTime dateOfBirth)
        {
            return !(dateOfBirth < DateTime.Today);
        }

        public static bool IsValidEmail(string email)
        {
            return string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return string.IsNullOrEmpty(phoneNumber) || string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, @"^\d{9}$");
        }

        public static bool IsValidNIP(string NIP)
        {
            return string.IsNullOrWhiteSpace(NIP) || string.IsNullOrEmpty(NIP) || !Regex.IsMatch(NIP, @"^\d{10}$");
        }

        public static bool IsPositiveNumber(string number)
        {
            int.TryParse(number, out int intNumber);
            return intNumber > 0;
        }

        public static bool IsNumberInRange(string number, int min, int max)
        {
            if (int.TryParse(number, out int intNumber))
            {
                return intNumber >= min && intNumber <= max;
            }
            return false; // jeśli błąd przy konwersji, uznaje że jest poza zakresem
        }
    }
}
