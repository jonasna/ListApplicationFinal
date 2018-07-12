using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ListApplicationFinal.Pages
{
    public partial class NavBarPage : NavigationPage
    {
        public NavBarPage()
        {
            InitializeComponent();
            SetBinding(ShouldNavigateProperty, new Binding("IsEnabled"));
        }

        public BindableProperty ShouldNavigateProperty = BindableProperty.Create(nameof(ShouldNavigate), typeof(bool), typeof(NavBarPage), true);

        public bool ShouldNavigate
        {
            get => (bool) GetValue(ShouldNavigateProperty);
            set => SetValue(ShouldNavigateProperty, value);
        }
    }
}
