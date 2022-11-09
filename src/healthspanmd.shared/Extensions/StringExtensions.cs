using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.shared.Extensions
{
    public static class StringExtensions
    {
        public static string ToE164PhoneNumberWithCountryCode(this string phoneNumber, string countryCode)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return null;

            var result = phoneNumber
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(".", "");

            if (!result.StartsWith($"+{countryCode}"))
            {
                result = $"+{countryCode}{result}";
            }

            return result;
        }

        public static string ToE164PhoneNumberInUS(this string phoneNumber)
        {
            return phoneNumber.ToE164PhoneNumberWithCountryCode("1");
        }

    }
}
