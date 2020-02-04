using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateAmazonPurchasesFromCSV.Services
{
    internal class CsvLoadItemsService : ILoadItemsService
    {
        public T[] LoadItems<T>(string fullPath) where T : class
        {
            FileHelperEngine<T> engine = new FileHelperEngine<T>();

            engine.Options.IgnoreFirstLines = 1;

            return engine.ReadFile(fullPath);
        }
    }
}
