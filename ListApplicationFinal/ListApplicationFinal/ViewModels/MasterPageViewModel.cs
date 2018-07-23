using Prism.Commands;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class MasterPageViewModel : VmBase
    {
        public MasterPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        private bool _isPresented;
        public bool IsPresented
        {
            get => _isPresented;
            set => SetProperty(ref _isPresented, value);
        }

        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand));

        private async void ExecuteNavigateCommand(string uri)
        {
            var navResult = await NavigationService.NavigateAsync("NavBarPage/" + uri);
            if (!navResult.Success)
                IsPresented = false;
        }
    }
}