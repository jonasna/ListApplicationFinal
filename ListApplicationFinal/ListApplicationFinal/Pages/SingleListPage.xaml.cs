using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace ListApplicationFinal.Pages
{
    public partial class SingleListPage : ContentPage
    {
        public SingleListPage()
        {
            InitializeComponent();
            ToDoListView.PropertyChanged += ToDoListView_PropertyChanged;
        }

        private void ToDoListView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Width" &&
                ToDoListView.Orientation == Orientation.Vertical &&
                ToDoListView.SwipeOffset != ToDoListView.Width)
            {
                ToDoListView.SwipeOffset = ToDoListView.SwipeThreshold = ToDoListView.Width;
            }
        }
    }
}
