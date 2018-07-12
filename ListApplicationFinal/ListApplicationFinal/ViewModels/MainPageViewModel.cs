using System;
using Prism.Commands;
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

        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand));

        private async void ExecuteNavigateCommand(string uri)
        {
            await NavigationService.NavigateAsync(new Uri(uri, UriKind.Relative));
        }
    }
}