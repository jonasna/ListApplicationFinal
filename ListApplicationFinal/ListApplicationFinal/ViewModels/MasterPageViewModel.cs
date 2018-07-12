using ListApplicationFinal.ViewModels.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class MasterPageViewModel : VmBase
    {
        private string _currentPage;
        private readonly SubscriptionToken _subscriptionToken;
        public MasterPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService, eventAggregator)
        {
            _subscriptionToken = EventAggregator.GetEvent<CurrentPageNameEvent>().Subscribe(HandleCurrentPageNameEvent);
        }

        private void HandleCurrentPageNameEvent(string pageName)
        {
            _currentPage = pageName;
        }

        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(ExecuteCommandName));

        private async void ExecuteCommandName(string uri)
        {
            var navResult = await NavigationService.NavigateAsync("NavBarPage/" + uri);

            if (!navResult.Success && _currentPage != null)
                await NavigationService.NavigateAsync("NavBarPage/" + _currentPage);
        }   

        public override void Destroy()
        {
            EventAggregator.GetEvent<CurrentPageNameEvent>().Unsubscribe(_subscriptionToken);
        }
    }
}