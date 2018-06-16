using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ClientApp {

    public sealed partial class SettingsPage : Page {

        public SettingsPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            Theme currentThemeSetting = App.settingsService.GetThemeSetting();
            ThemeComboBox.SelectedItem = ThemeComboBox.Items.Cast<ComboBoxItem>().Where(i => currentThemeSetting.ToString().Contains(i.Tag.ToString())).First();
        }

        private void SelectTheme(object sender, SelectionChangedEventArgs e) {
            App.settingsService.SetThemeSetting( ((ComboBoxItem)ThemeComboBox.SelectedItem).Tag );

            // TODO: Change theme on the fly
            //if (App.settingsService.GetThemeSetting() == Theme.Light) {
            //    App.Current.RequestedTheme = ApplicationTheme.Light;

            //} else if (App.settingsService.GetThemeSetting() == Theme.Dark) {
            //    App.Current.RequestedTheme = ApplicationTheme.Dark;

            //} else {
            //    var DefaultTheme = new Windows.UI.ViewManagement.UISettings();
            //    var uiTheme = DefaultTheme.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background).ToString();
            //    if (uiTheme == "#FF000000") {
            //        App.Current.RequestedTheme = ApplicationTheme.Dark;
            //    } else if (uiTheme == "#FFFFFFFF") {
            //        App.Current.RequestedTheme = ApplicationTheme.Light;
            //    }
            //}
        }
    }

}
