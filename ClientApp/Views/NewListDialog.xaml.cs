using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp.Views {
    public sealed partial class NewListDialog : ContentDialog {
        public NewListDialog() {
            this.InitializeComponent();
        }

        private void Save(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            string value = ListName.Text;
            if (value != "") {
                // TODO: implement making new list
                //List list = new List() { Name = value };
                //App.dataService.NewList(list);
                base.Hide();
            } else {
                if (args != null)
                    args.Cancel = true;
                Error.Text = "Name cannot be empty";
            }
        }

        private void Cancel(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            base.Hide();
        }

        private void OnTextBoxKeyDown(object sender, KeyRoutedEventArgs e) {
            Error.Text = "";
            if (e.Key == Windows.System.VirtualKey.Enter)
                Save(this, null);
        }

        public void ShowAsync() {
            base.ShowAsync();
        }
    }
}
