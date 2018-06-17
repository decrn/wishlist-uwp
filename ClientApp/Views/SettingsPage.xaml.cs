using System.Linq;
using Windows.UI.Xaml;
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

            BackgroundTaskToggleSwitch.IsOn = App.settingsService.GetBackgroundTaskEnabledSetting();
        }

        private void SelectTheme(object sender, SelectionChangedEventArgs e) {
            App.settingsService.SetThemeSetting( ((ComboBoxItem)ThemeComboBox.SelectedItem).Tag );
        }

        private void ToggleBackgroundTask(object sender, RoutedEventArgs e) {
            App.settingsService.SetBackgroundTaskEnabledSetting(BackgroundTaskToggleSwitch.IsOn);
        }
    }

}
