using System.Linq;
using System.Collections.Generic;
using ValidateAmazonPurchasesFromCSV.Models;
using System;
using System.Text;

namespace ValidateAmazonPurchasesFromCSV.Services
{
    internal class ItemScanner : IItemScanner
    {
        public IEnumerable<ScannedItem> Scan(AmazonOrderItem[] amazonOrderItems, ChaseCreditLineItem[] creditLineItems)
        {
            List<ScannedItem> scannedItems = new List<ScannedItem>();

            IEnumerable<ChaseCreditLineItem> amazonCreditLineItems =
                from lineItem in creditLineItems
                where (lineItem.Description.IndexOf("Amazon", StringComparison.OrdinalIgnoreCase) > -1 ||
                       lineItem.Description.IndexOf("AMZN", StringComparison.OrdinalIgnoreCase) > -1) &&
                       lineItem.Description.IndexOf("Kindle", StringComparison.OrdinalIgnoreCase) == -1 &&
                       lineItem.Description.IndexOf("Audible", StringComparison.OrdinalIgnoreCase) == -1
                select lineItem;

            foreach (ChaseCreditLineItem amazonLineItem in amazonCreditLineItems)
            {
                // invert the amount as the credit report brings them in as negative
                decimal cost = -amazonLineItem.Amount;

                ScannedItem scannedItem = LocateScannedItemBasedOnCost(amazonOrderItems, amazonLineItem, cost);

                if (scannedItem == null)
                {
                    // Populate with some data, but most importantely indicate that the transaction(s) wasn't found
                    scannedItem = new ScannedItem()
                    {
                        ItemTotal = cost,
                        PostingDate = amazonLineItem.PostingDate,
                        Title = amazonLineItem.Description,
                        WasFound = false
                    };
                }

                scannedItems.Add(scannedItem);
            }

            return scannedItems;
        }

        private static ScannedItem LocateScannedItemBasedOnCost(AmazonOrderItem[] amazonOrderItems, ChaseCreditLineItem amazonLineItem, decimal cost)
        {
            IEnumerable<AmazonOrderItem> orders =
                from orderItem in amazonOrderItems
                where orderItem.ItemTotal == cost && !orderItem.IsUsed && orderItem.OrderDate <= amazonLineItem.PostingDate
                orderby orderItem.OrderDate
                select orderItem;

            AmazonOrderItem order = orders.FirstOrDefault();
            ScannedItem scannedItem = null;

            if (order != null)
            {
                order.IsUsed = true;

                scannedItem = new ScannedItem()
                {
                    ItemTotal = cost,
                    OrderDate = order.OrderDate,
                    PostingDate = amazonLineItem.PostingDate,
                    Title = order.Title,
                    WasFound = true
                };
            }
            else
            {
                // Check for suscription savings - appears that these appear w/ + Gift Card in the description (HACK)
                IEnumerable<AmazonOrderItem> subscriptionOrders =
                    from orderItem in amazonOrderItems
                    where orderItem.PaymentInstrumentType.Contains("Gift") && !orderItem.IsUsed && orderItem.OrderDate <= amazonLineItem.PostingDate &&
                        ((Math.Round(orderItem.ItemSubtotal * (decimal)0.9595 + orderItem.ItemSubtotalTax, 2) == cost) || (Math.Round(orderItem.ItemSubtotal * (decimal)0.8595 + orderItem.ItemSubtotalTax, 2) == cost))
                    orderby orderItem.OrderDate
                    select orderItem;

                order = subscriptionOrders.FirstOrDefault();

                if (order != null)
                {
                    order.IsUsed = true;

                    scannedItem = new ScannedItem()
                    {
                        ItemTotal = cost,
                        OrderDate = order.OrderDate,
                        PostingDate = amazonLineItem.PostingDate,
                        Title = order.Title,
                        WasFound = true
                    };
                }
                else
                {
                    // Check for aggregate orders on the same day
                    List<IGrouping<DateTime, AmazonOrderItem>> orderGroups = new List<IGrouping<DateTime, AmazonOrderItem>>();

                    IEnumerable<IGrouping<DateTime, AmazonOrderItem>> ordersByDate = amazonOrderItems.GroupBy((o) => o.OrderDate);

                    foreach (IGrouping<DateTime, AmazonOrderItem> orderByDate in ordersByDate)
                    {
                        if (orderByDate.Count() > 1)
                        {
                            decimal total = orderByDate.Sum((o) => o.ItemTotal);

                            if (total == cost)
                            {
                                orderGroups.Add(orderByDate);
                            }
                        }
                    }

                    if (orderGroups.Any())
                    {
                        IOrderedEnumerable<IGrouping<DateTime, AmazonOrderItem>> orderGroupsOrderedByDate = orderGroups.OrderBy((groupedItem) => groupedItem.Key);
                        IGrouping<DateTime, AmazonOrderItem> orderGroup = orderGroupsOrderedByDate.First();

                        StringBuilder builder = new StringBuilder();

                        foreach (AmazonOrderItem items in orderGroup)
                        {
                            builder.Append(items.Title);
                            builder.Append(" ");
                            items.IsUsed = true;
                        }

                        scannedItem = new ScannedItem()
                        {
                            ItemTotal = cost,
                            OrderDate = orderGroup.Key,
                            PostingDate = amazonLineItem.PostingDate,
                            Title = builder.ToString(),
                            WasFound = true
                        };
                    }
                }
            }

            return scannedItem;
        }
    }
}
