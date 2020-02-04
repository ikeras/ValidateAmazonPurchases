using System;

namespace ValidateAmazonPurchasesFromCSV.Models
{
    public class ScannedItem
    {
        public decimal ItemTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string Title { get; set; }
        public bool WasFound { get; set; }
    }
}
