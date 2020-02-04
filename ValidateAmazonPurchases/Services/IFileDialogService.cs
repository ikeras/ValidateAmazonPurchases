using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateAmazonPurchasesFromCSV.Services
{
    internal interface IFileDialogService
    {
        string ShowOpenFileDialog(string filter);
    }
}
