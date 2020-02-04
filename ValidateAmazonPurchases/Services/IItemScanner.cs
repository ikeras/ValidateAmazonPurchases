using System.Collections.Generic;
using ValidateAmazonPurchasesFromCSV.Models;

namespace ValidateAmazonPurchasesFromCSV.Services
{
    internal interface IItemScanner
    {
        IEnumerable<ScannedItem> Scan(AmazonOrderItem[] amazonOrderItems, ChaseCreditLineItem[] creditLineItems);
    }
}
