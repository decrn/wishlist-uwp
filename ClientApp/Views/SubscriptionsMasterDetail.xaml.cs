using ClientApp.Models;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SubscriptionMasterDetail : Page
    {
        public ICollection<List> Lists { get; set; }

        public SubscriptionMasterDetail() {
            // TODO: Use Viewmodels?
            Lists = App.dataService.GetSubscribedLists();
            InitializeComponent();
        }

    }
}
