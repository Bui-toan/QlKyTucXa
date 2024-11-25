using System;
using System.Globalization;

namespace QlKyTucXa.Utils
{
    public static class CurrencyFormatter
    {

        public static string FormatCurrency(decimal amount, string cultureCode = "vi-vn")
        {
            try
            {
                CultureInfo culture = new CultureInfo(cultureCode);
                return string.Format(culture, "{0:C}", amount);
            }
            catch (CultureNotFoundException)
            {
                throw new ArgumentException($"Invalid culture code: {cultureCode}");
            }
        }
    }
}
