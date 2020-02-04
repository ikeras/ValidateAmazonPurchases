using Microsoft.Win32;

namespace ValidateAmazonPurchasesFromCSV.Services
{
    internal class WpfFileDialogService : IFileDialogService
    {
        public string ShowOpenFileDialog(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = filter
            };

            string chosenFilename = null;

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                chosenFilename = openFileDialog.FileName;
            }

            return chosenFilename;
        }
    }
}
