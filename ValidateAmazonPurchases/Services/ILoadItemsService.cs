using System.Threading.Tasks;

namespace ValidateAmazonPurchasesFromCSV.Services
{
    internal interface ILoadItemsService
    {
        T[] LoadItems<T>(string fullPath) where T : class;
    }
}
