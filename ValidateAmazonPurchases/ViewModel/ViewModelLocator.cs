using GalaSoft.MvvmLight.Ioc;
using ValidateAmazonPurchasesFromCSV.Services;

namespace ValidateAmazonPurchasesFromCSV.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<IItemScanner, ItemScanner>();
            SimpleIoc.Default.Register<ILoadItemsService, CsvLoadItemsService>();
            SimpleIoc.Default.Register<IFileDialogService, WpfFileDialogService>();
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get => SimpleIoc.Default.GetInstance<MainViewModel>();
        }
        
        public static void Cleanup()
        {
        }
    }
}