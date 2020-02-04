using FileHelpers;
using System;

namespace ValidateAmazonPurchasesFromCSV.Converters
{
    /// <summary>
    /// Converts from a price of the form $35.00, to a decimal 35.00
    /// </summary>
    internal class MoneyConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            if (!from.StartsWith("$"))
            {
                throw new FormatException($"{from} does not begin with a $");
            }

            string value = from.Substring(1);
            return decimal.Parse(value);
        }
    }
}
