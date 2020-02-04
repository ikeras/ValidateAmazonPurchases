using FileHelpers;
using System;
using ValidateAmazonPurchasesFromCSV.Converters;

namespace ValidateAmazonPurchasesFromCSV.Models
{
#pragma warning disable CS0649
    [DelimitedRecord(",")]
    internal class AmazonOrderItem
    {
        [FieldConverter(ConverterKind.Date, "MM/dd/yy")]
        public DateTime OrderDate;
        public string OrderId;
        [FieldQuoted]
        public string Title;
        public string Category;
        public string InvoiceNumber;
        public string UNSPSCCode;
        public string Website;
        public string ReleaseDate;
        public string Condition;
        [FieldQuoted(QuoteMode.OptionalForRead)]
        public string Seller;
        [FieldQuoted(QuoteMode.OptionalForRead)]
        public string SellerCredentials;
        [FieldConverter(typeof(MoneyConverter))]
        public decimal ListPrice;
        [FieldConverter(typeof(MoneyConverter))]
        public decimal PurchasePrice;
        public int Quantity;
        public string PaymentInstrumentType;
        public string PurchaseOrderNumber;
        public string POLineNumber;
        public string OrderingCustomerEmail;
        [FieldConverter(ConverterKind.Date, "MM/dd/yy")]
        public DateTime? ShipmentDate;
        public string ShippingAddressName;
        public string ShippingAddressStreet1;
        public string ShippingAddressStreet2;
        public string ShippingAddressCity;
        public string ShippingAddressState;
        public string ShippingAddressZip;
        public string OrderStatus;
        [FieldQuoted]
        public string CarrierNameAndTrackingNumber;
        [FieldConverter(typeof(MoneyConverter))]
        public decimal ItemSubtotal;
        [FieldConverter(typeof(MoneyConverter))]
        public decimal ItemSubtotalTax;
        [FieldConverter(typeof(MoneyConverter))]
        public decimal ItemTotal;
        public string TaxExemptApplied;
        public string TaxExamptType;
        public string ExemptionOptOut;
        public string BuyerName;
        public string Currency;
        public string GroupName;

        [FieldHidden]
        public bool IsUsed;
    }
#pragma warning restore CS0649
}
