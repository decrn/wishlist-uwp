using ClientApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientApp {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SubscriptionMasterDetail : Page
    {
        private ListViewModel _lastSelectedList;
        public UserViewModel User { get; set; }

        public SubscriptionMasterDetail() {
            this.InitializeComponent();
            this.User = new UserViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            var subscriptions = MasterListView.ItemsSource as List<ListViewModel>;

            if (subscriptions == null) {
                subscriptions = new List<ListViewModel>();

                // TODO: Fix this crappy way for loading lists
                if (User.Subscriptions.Count < 1)
                    User.Subscriptions = App.dataService.GetSubscribedLists().Select(l => ListViewModel.FromList(l)).ToList();

                foreach (var sub in User.Subscriptions) {
                    subscriptions.Add(ListViewModel.FromList(sub));
                }

                MasterListView.ItemsSource = subscriptions;
            }

            // Keep track of where the user was browsing, so they don't have to scroll down again every time they go back to the master view
            if (e.Parameter != null && e.Parameter != "") {
                // Parameter is list ID
                var id = (int)e.Parameter;
                _lastSelectedList =
                    subscriptions.Where((list) => list.ListId == id).FirstOrDefault();
            }

            

            UpdateForVisualState(AdaptiveStates.CurrentState);

            // Don't play a content transition for first item load.
            // Sometimes, this content will be animated as part of the page transition.
            DisableContentTransitions();
        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e) {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState = null) {
            var isNarrow = newState == NarrowState;

            if (isNarrow && oldState == DefaultState && _lastSelectedList != null) {
                // Resize down to the detail item. Don't play a transition.
                Frame.Navigate(typeof(SubscriptionDetailPage), _lastSelectedList.ListId, new SuppressNavigationTransitionInfo());
            }

            EntranceNavigationTransitionInfo.SetIsTargetElement(MasterListView, isNarrow);
            if (DetailContentPresenter != null) {
                EntranceNavigationTransitionInfo.SetIsTargetElement(DetailContentPresenter, !isNarrow);
            }
        }

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e) {
            var clickedList = (ListViewModel)e.ClickedItem;
            _lastSelectedList = clickedList;

            if (AdaptiveStates.CurrentState == NarrowState) {
                // Use "drill in" transition for navigating from master list to detail view
                Frame.Navigate(typeof(SubscriptionDetailPage), clickedList.ListId, new DrillInNavigationTransitionInfo());
            } else {
                // Play a refresh animation when the user switches detail items.
                EnableContentTransitions();
            }
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e) {
            // Assure we are displaying the correct item. This is necessary in certain adaptive cases.
            MasterListView.SelectedItem = _lastSelectedList;
        }

        private void EnableContentTransitions() {
            DetailContentPresenter.ContentTransitions.Clear();
            DetailContentPresenter.ContentTransitions.Add(new EntranceThemeTransition());
        }

        private void DisableContentTransitions() {
            if (DetailContentPresenter != null) {
                DetailContentPresenter.ContentTransitions.Clear();
            }
        }

    }
}
