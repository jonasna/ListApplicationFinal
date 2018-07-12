using ListApplicationFinal.ViewModels.Events;
using ListApplicationFinal.ViewModels.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class MasterPageViewModel : VmBase, IHandlePageNameChange
    {
        private string _currentPage;

        public MasterPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public void HandlePageNameChange(string pageName)
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
    }
}