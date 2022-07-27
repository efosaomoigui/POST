using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GIGLS.Services.Implementation.Utility
{
    public static class InputsValidator
    {
        private const string emailExpre = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        private const string phoneExpre = @"/((^090)([23589]))|((^070)([1-9]))|((^080)([2-9]))|((^081)([0-9]))(\d{7})|((^091)([0-9]))(\d{7})/";
        public static bool ValidateEmail(string email)
        {
            var result = Regex.IsMatch(email, emailExpre);
            return result;
        }

        public static bool ValidatePhoneNumber(string number)
        {
            var result = Regex.IsMatch(number, phoneExpre);
            return result;
        }
    }
}
