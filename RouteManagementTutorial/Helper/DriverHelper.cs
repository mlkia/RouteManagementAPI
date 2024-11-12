using RouteManagementTutorial.Entities;
using System.Text.RegularExpressions;
using ZstdSharp.Unsafe;

namespace RouteManagementTutorial.Helper
{
    public class DriverHelper
    {
        /// <summary>
        /// Regular expression pattern for validating a 10-digit phone number.
        /// </summary>
        /// <remarks>
        /// This pattern ensures that the phone number consists of exactly 10 digits and no other characters or spaces.
        /// </remarks>
        public const string PhoneNumberPattern = @"^([0-9]{10})$"; //or @"^\d{10}$"

        public const string NationalNumberPattern = @"^(19[0-9]{2}|20[0-9]{2})(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])$";

        public const string PasswordPattern = @"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d!#$%&'()*+,-./:;<=>?@[\]^_`{|}~ ]{8,20}$";

        /// <summary>
        /// Validates the email address.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns><c>true</c> if the email address is not null or empty and contains an "@" symbol; otherwise, <c>false</c>.</returns>
        public static bool EmailValidation(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                return false;
            }

            return true;
        }

        public static bool PasswordValidation(string password)
        {
            if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, PasswordPattern))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates the phone number using a predefined pattern.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <returns><c>true</c> if the phone number is not null or empty and matches the phone number pattern; otherwise, <c>false</c>.</returns>
        public static bool PhoneNumberValidation(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || !Regex.IsMatch(phoneNumber, PhoneNumberPattern))
            {
                return false;
            }
            return true;
        }

    }
}
