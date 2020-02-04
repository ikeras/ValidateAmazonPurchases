using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ValidateAmazonPurchasesFromCSV.Models;
using ValidateAmazonPurchasesFromCSV.Services;

namespace ValidateAmazonPurchasesFromCSV.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string pathToAmazonOrdersCSV;
        private string pathToCreditcardCSV;
        private ObservableCollection<ScannedItem> scannedItems;

        public RelayCommand<string> BrowseCommand { get; private set; }
        public RelayCommand ProcessCommand { get; private set; }

        public bool IsReadyForCalculation
        {
            get => this.PathToCreditcardCSV != null && this.PathToAmazonOrdersCSV != null;
        }

        public string PathToAmazonOrdersCSV
        {
            get => this.pathToAmazonOrdersCSV;
            set
            {
                this.Set(nameof(this.PathToAmazonOrdersCSV), ref this.pathToAmazonOrdersCSV, value);
                this.RaisePropertyChanged(nameof(this.IsReadyForCalculation));
            }
        }

        public string PathToCreditcardCSV
        {
            get => this.pathToCreditcardCSV;
            set
            {
                this.Set(nameof(this.PathToCreditcardCSV), ref this.pathToCreditcardCSV, value);
                this.RaisePropertyChanged(nameof(this.IsReadyForCalculation));
            }
        }

        public ObservableCollection<ScannedItem> ScannedItems
        {
            get => this.scannedItems;
            set
            {
                this.Set(nameof(ScannedItems), ref this.scannedItems, value);
            }
        }

        public MainViewModel()
        {
            this.BrowseCommand = new RelayCommand<string>(this.OnBrowseCommand);
            this.ProcessCommand = new RelayCommand(this.OnProcessCommand);
        }

        private void OnBrowseCommand(string target)
        {
            IFileDialogService fileDialogService = SimpleIoc.Default.GetInstance<IFileDialogService>();

            string filename = fileDialogService.ShowOpenFileDialog("CSV files (.csv)|*.csv");

            if (filename != null)
            {
                switch (target)
                {
                    case "Creditcard":
                        this.PathToCreditcardCSV = filename;
                        break;
                    case "Orders":
                        this.PathToAmazonOrdersCSV = filename;
                        break;
                }
            }
        }

        private async void OnProcessCommand()
        {
            ILoadItemsService loadItemsService = SimpleIoc.Default.GetInstance<ILoadItemsService>();

            Task<ChaseCreditLineItem[]> loadCreditLineItemTask = Task.Run(() => loadItemsService.LoadItems<ChaseCreditLineItem>(this.PathToCreditcardCSV));
            Task<AmazonOrderItem[]> loadAmazonOrdersTask = Task.Run(() => loadItemsService.LoadItems<AmazonOrderItem>(this.PathToAmazonOrdersCSV));

            await Task.WhenAll(loadCreditLineItemTask, loadAmazonOrdersTask);

            IItemScanner scanner = SimpleIoc.Default.GetInstance<IItemScanner>();

            IEnumerable<ScannedItem> scannedItems = await Task.Run(() => scanner.Scan(loadAmazonOrdersTask.Result, loadCreditLineItemTask.Result));

            this.ScannedItems = new ObservableCollection<ScannedItem>(scannedItems);
        }
    }
}