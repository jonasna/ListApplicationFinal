using System.ComponentModel;
using ListApplicationFinal.ViewModels;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace ListApplicationFinal.Pages
{
    public partial class SingleListPage : ContentPage
    {
        private SingleListPageViewModel ViewModel => (SingleListPageViewModel) BindingContext;

        public SingleListPage()
        {
            InitializeComponent();
            ToDoListView.PropertyChanged += SwipeConfig_OnWidthChanged;
            ToDoListView.PropertyChanged += FooterSize_OnHeightChanged;
            NavigationPage.SetHasBackButton(this, false);
        }

        private void FooterSize_OnHeightChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Height")
            {
                if (ToDoListView.FooterSize != 90)
                {
                    if (ViewModel.IsRefreshing && ToDoListView.FooterSize != Application.Current.MainPage.Height)
                        ViewModel.NotifyRefresh();
                }
            }
        }

        private void SwipeConfig_OnWidthChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Width" &&
                ToDoListView.Orientation == Orientation.Vertical &&
                ToDoListView.SwipeOffset != ToDoListView.Width)
            {
                ToDoListView.SwipeOffset = ToDoListView.SwipeThreshold = ToDoListView.Width;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            ViewModel.NavigateBack();
            return true;
        }
    }
}
