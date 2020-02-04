using FileHelpers;
using System;
using ValidateAmazonPurchasesFromCSV.Converters;

namespace ValidateAmazonPurchasesFromCSV.Models
{
#pragma warning disable CS0649
    [DelimitedRecord(",")]
    internal class ChaseCreditLineItem
    {
        [FieldQuoted]
        [FieldConverter(typeof(DateMultiFormatExtendedConverter), "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy")]
        public DateTime TransactionDate;
        [FieldQuoted]
        [FieldConverter(typeof(DateMultiFormatExtendedConverter), "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy")]
        public DateTime PostingDate;
        [FieldQuoted]
        public string Description;
        [FieldQuoted]
        public string TransactionCategory;
        [FieldQuoted]
        public string Type;
        [FieldQuoted]
        public decimal Amount;
    }
#pragma warning restore CS0649
}
