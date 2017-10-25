using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClientApp.Models;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ClientApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private ObservableCollection<List> _lists = new ObservableCollection<List>();

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            HttpClient client = new HttpClient();
            var json = await client.GetStringAsync(new Uri("http://localhost:64042/api/Lists"));
            _lists = JsonConvert.DeserializeObject<ObservableCollection<List>>(json);
            list.ItemsSource = _lists;
        }

    }
}
