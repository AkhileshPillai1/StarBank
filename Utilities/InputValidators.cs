using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StarBank.Utilities
{
    internal class InputValidators
    {
        public static bool validateEmail(string email)
        {
            try
            {
                // Regular expression for validating an Email
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

                // Return true if the email matches the pattern, false otherwise
                return regex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool validatePassword(string? password) { 
            if(string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                return false;
            }
            return true;
        }

    }
}
