using System.Text.RegularExpressions;

namespace CarRentalAPI.Global_Classes
{
    public class clsValidation
    {
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            const string pattern =@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9\s]).{8,}$";
                

            return Regex.IsMatch(password, pattern);
        }

        public static bool ValidateEmail(string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return false;

            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Email);
        }

        public static bool ValidateInteger(string Number)
        {
            if (string.IsNullOrEmpty(Number))
                return false;

            var pattern = @"^[0-9]*$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }

        public static bool ValidateFloat(string Number)
        {
            if (string.IsNullOrEmpty(Number))
                return false;

            var pattern = @"^[0-9]*(?:\.[0-9]*)?$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }

        public static bool IsNumber(string Number)
        {
            if (string.IsNullOrEmpty(Number))
                return false;

            return (ValidateInteger(Number) || ValidateFloat(Number));
        }

    }
}
