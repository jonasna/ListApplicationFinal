using System.Collections.ObjectModel;
using ListApplicationFinal.DataServices;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class LoginPageViewModel : VmBase
    {
        private const string SettingsSavedTest = "Your settings have been saved";
        private const string SettingsNotSavedText = "Your settings have not been saved";

        private readonly IApplicationUserService _applicationUserService;
        public LoginPageViewModel(INavigationService navigationService, IApplicationUserService applicationUserService) 
            : base(navigationService)
        {
            Title = "Login Page";
            _applicationUserService = applicationUserService;           
        }

        private string _welcomeText;
        public string WelcomeText
        {
            get => _welcomeText;
            set => SetProperty(ref _welcomeText, value);
        }

        private bool _isValid;    
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            WelcomeText = _applicationUserService.IsValid ? SettingsSavedTest : SettingsNotSavedText;
            IsValid = _applicationUserService.IsValid;
        }
    }
}