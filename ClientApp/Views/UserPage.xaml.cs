using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClientApp {

    public sealed partial class UserPage : Page {

        public UserPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            Id.Text = App.dataService.GetCurrentUser().Id;
            Email.Text = App.dataService.GetCurrentUser().Email;
            NameHeader.Text = App.dataService.GetCurrentUser().GetFullName();
        }
    }

}
