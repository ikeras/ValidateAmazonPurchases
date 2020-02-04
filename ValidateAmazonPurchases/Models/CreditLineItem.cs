using FileHelpers;
using System;
using ValidateAmazonPurchasesFromCSV.Converters;

namespace ValidateAmazonPurchasesFromCSV.Models
{
#pragma warning disable CS0649
    [DelimitedRecord(",")]
    internal class CreditLineItem
    {
        [FieldQuoted]
        public string TransactionId;
        [FieldQuoted]
        [FieldConverter(typeof(DateMultiFormatExtendedConverter), "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy")]
        public DateTime PostingDate;
        [FieldQuoted]
        [FieldConverter(typeof(DateMultiFormatExtendedConverter), "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy")]
        public DateTime EffectiveDate;
        [FieldQuoted]
        public string TransactionType;
        [FieldQuoted]
        public decimal Amount;
        [FieldQuoted]
        public string CheckNumber;
        [FieldQuoted]
        public string ReferenceNumber;
        [FieldQuoted]
        public string Description;
        [FieldQuoted]
        public string TransactionCategory;
        [FieldQuoted]
        public string Type;
        [FieldQuoted]
        public decimal Balance;
    }
#pragma warning restore CS0649
}
