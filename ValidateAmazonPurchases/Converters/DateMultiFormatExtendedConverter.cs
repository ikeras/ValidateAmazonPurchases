using System;
using System.Globalization;
using FileHelpers;

namespace ValidateAmazonPurchasesFromCSV.Converters
{
    internal class DateMultiFormatExtendedConverter : ConverterBase
    {
        private readonly string[] formats;

        public DateMultiFormatExtendedConverter(string format1, string format2, string format3, string format4) => this.formats = new string[] { format1, format2, format3, format4 };

        public override object StringToField(string from)
        {
            foreach (string format in this.formats)
            {
                if (DateTime.TryParseExact(from, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    return dateTime;
                }
            }

            throw new ConvertException(from, typeof(DateTime));
        }
    }
}
