using ClientApp.Models;
using ClientApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Printing;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OwnedMasterDetail : Page {

        public ICollection<List> Lists{ get; set; }
        private List CurrentList;

        string[] CategorySuggestions = new string[] { "Toys", "Homewares", "Gadgets", "Vouchers", "Varia" };

        public OwnedMasterDetail() {
            // TODO: Use Viewmodels?
            Lists = App.dataService.GetOwnedLists();
            InitializeComponent();

            // return the full detail list when opening detail panel
            MasterDetail.MapDetails = (selected) => {
                CurrentList = App.dataService.GetList(((List) selected).ListId);
                return CurrentList;
            };
        }

        public void SelectList(List list) {
            if (list == null) MasterDetail.SelectedItem = null;
            else MasterDetail.SelectedItem = Lists.First((l) => l.ListId == list.ListId);
        }

        private async void NewList(object sender, RoutedEventArgs e) {
            NewListDialog dialog = new NewListDialog();
            dialog.ShowAsync();
        }

        private void Save(object sender, RoutedEventArgs e) {
            // check if list is new (no id) and use save or create new route
        }

        private void Send(object sender, RoutedEventArgs e) {

        }

        private void Delete(object sender, RoutedEventArgs e) {

        }

        private void AddNewWish(object sender, RoutedEventArgs e) {

        }

        private void AddInvite(object sender, RoutedEventArgs e) {

        }

        private void Category_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
                sender.ItemsSource = CategorySuggestions;
            }
        }

        private void Category_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
            sender.Text = args.SelectedItem.ToString();
        }

        private void Category_GotFocus(object sender, RoutedEventArgs e) {
            AutoSuggestBox box = (AutoSuggestBox)sender;
            box.ItemsSource = CategorySuggestions;
            box.IsSuggestionListOpen = true;
        }


        #region Printing

        private PrintManager printMan;
        private PrintDocument printDoc;
        private IPrintDocumentSource printDocSource;

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            // Register for PrintTaskRequested event
            printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested += PrintTaskRequested;

            // Build a PrintDocument and register for callbacks
            printDoc = new PrintDocument();
            printDocSource = printDoc.DocumentSource;
            printDoc.Paginate += Paginate;
            printDoc.GetPreviewPage += GetPreviewPage;
            printDoc.AddPages += AddPages;
        }

        private async void Export(object sender, RoutedEventArgs e) {

            //var messageDialog = new MessageDialog("This functionality hasn't been added yet.");
            //messageDialog.ShowAsync();

            if (Windows.Graphics.Printing.PrintManager.IsSupported()) {
                try {
                    // Show print UI
                    await Windows.Graphics.Printing.PrintManager.ShowPrintUIAsync();

                } catch {
                    // Printing cannot proceed at this time
                    ContentDialog noPrintingDialog = new ContentDialog() {
                        Title = "Printing error",
                        Content = "\nSorry, printing can' t proceed at this time.", PrimaryButtonText = "OK"
                    };
                    await noPrintingDialog.ShowAsync();
                }
            } else {
                // Printing is not supported on this device
                ContentDialog noPrintingDialog = new ContentDialog() {
                    Title = "Printing not supported",
                    Content = "\nSorry, printing is not supported on this device.", PrimaryButtonText = "OK"
                };
                await noPrintingDialog.ShowAsync();
            }
        }

        private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args) {
            // Create the PrintTask.
            // Defines the title and delegate for PrintTaskSourceRequested
            var printTask = args.Request.CreatePrintTask("Print", PrintTaskSourceRequrested);

            // Handle PrintTask.Completed to catch failed print jobs
            printTask.Completed += PrintTaskCompleted;
        }

        private void PrintTaskSourceRequrested(PrintTaskSourceRequestedArgs args) {
            // Set the document source.
            args.SetSource(printDocSource);
        }

        private void Paginate(object sender, PaginateEventArgs e) {
            // As I only want to print one Rectangle, so I set the count to 1
            printDoc.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }

        private void GetPreviewPage(object sender, GetPreviewPageEventArgs e) {
            // Provide a UIElement as the print preview.
            var element = this.MasterDetail.ItemsPanelRoot;
            //printDoc.SetPreviewPage(e.PageNumber, element);
        }

        private void AddPages(object sender, AddPagesEventArgs e) {
            //printDoc.AddPage(DetailsView);

            // Indicate that all of the print pages have been provided
            printDoc.AddPagesComplete();
        }

        private async void PrintTaskCompleted(PrintTask sender, PrintTaskCompletedEventArgs args) {
            // Notify the user when the print operation fails.
            if (args.Completion == PrintTaskCompletion.Failed) {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    ContentDialog noPrintingDialog = new ContentDialog() {
                        Title = "Printing error",
                        Content = "\nSorry, failed to print.",
                        PrimaryButtonText = "OK"
                    };
                    await noPrintingDialog.ShowAsync();
                });
            }
        }

        #endregion
    }
}
