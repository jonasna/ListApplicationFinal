using Prism.Events;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class MainPageViewModel : VmBase
    {
        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Main Page";
        }
    }
}