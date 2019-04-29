using System;
using System.Linq;

namespace BrainNotFound.Paper.Services
{
    public static class PhoneNumber
    {
        public static string CalculatePhoneNumber(string number)
        {
            string phoneNumber;

            if (number == String.Empty)
            {
                return null;
            }

            number = new string((from c in number
                                 where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)
                                 select c).ToArray());

            phoneNumber = "(" + number.Substring(0, 3) + ") " + number.Substring(3, 3) + "-" + number.Substring(6, 4);
            
            return phoneNumber;
        }
    }
}