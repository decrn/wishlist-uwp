using System;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Printing;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;

// Source: https://stackoverflow.com/a/39140069

namespace ClientApp.Helpers {
    public class PrintablePage : Page {

        private PrintManager printMan;
        private PrintDocument printDoc;
        private IPrintDocumentSource printDocSource;

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            // Register for PrintTaskRequested event
            printMan = PrintManager.GetForCurrentView();
            // TODO: sometimes crashes when logging in
            printMan.PrintTaskRequested += PrintTaskRequested;

            // Build a PrintDocument and register for callbacks
            printDoc = new PrintDocument();
            printDocSource = printDoc.DocumentSource;
            printDoc.Paginate += Paginate;
            printDoc.GetPreviewPage += GetPreviewPage;
            printDoc.AddPages += AddPages;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            printMan.PrintTaskRequested -= PrintTaskRequested;
        }

        protected async void Export(object sender, RoutedEventArgs e) {

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
            printDoc.SetPreviewPage(e.PageNumber, this);
        }

        private void AddPages(object sender, AddPagesEventArgs e) {
            printDoc.AddPage(this);

            // Indicate that all of the print pages have been provided
            printDoc.AddPagesComplete();
        }

        private async void PrintTaskCompleted(PrintTask sender, PrintTaskCompletedEventArgs args) {
            // Notify the user when the print operation fails.
            if (args.Completion == PrintTaskCompletion.Failed) {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => {
                    ContentDialog noPrintingDialog = new ContentDialog() {
                        Title = "Printing error",
                        Content = "\nSorry, failed to print.",
                        PrimaryButtonText = "OK"
                    };
                    await noPrintingDialog.ShowAsync();
                });
            }
        }

    }
}
