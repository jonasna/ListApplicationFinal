using Prism.Events;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class MainPageViewModel : VmBase
    {
        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService, eventAggregator)
        {
            Title = "Main Page";
        }
    }
}