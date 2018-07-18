using System.Threading.Tasks;
using DialogServices.Service;
using ListApplicationFinal.DataServices;
using ListApplicationFinal.ViewModels.Interfaces;
using Prism.Commands;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class LoginPageViewModel : VmBase, IConfirmNavigation, INotifyPageNameChange
    {
        private const string SettingsSavedText = "Your settings have been saved";
        private const string SettingsNotSavedText = "Your settings have not been saved";
        private const string ErrorMsgText = "Fill out the form before continuing";

        private const string DialogQuestion =
            "Your settings have been saved succesfully. Would you like to navigate to main page?";

        private const string MainPage = "../MainPage";

        private readonly IApplicationUserService _applicationUserService;
        private readonly IDialogService _dialogService;

        public bool ShouldNotify { get; set; } // INotifyPageName

        private Task<bool> ShouldNavigateToMainPage =>
            _dialogService.QuestionDialog(DialogQuestion, "Question", "Yes", "No");

        public LoginPageViewModel(INavigationService navigationService,
            IApplicationUserService applicationUserService,
            IDialogService dialogService) 
            : base(navigationService)
        {
            Title = "Login Page";
            ShouldNotify = true;

            _applicationUserService = applicationUserService;
            _dialogService = dialogService;
        }

        private bool _firstLoad = false;
        public bool FirstLoad
        {
            get => _firstLoad;
            set => SetProperty(ref _firstLoad, value);
        }

        private string _welcomeText;
        public string WelcomeText
        {
            get => _welcomeText;
            set
            {
                if (!SetProperty(ref _welcomeText, value) && !_applicationUserService.IsValid)
                {
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool _isValid;    // Determines text color in the view
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, OnNamePropertySet);
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value, OnNamePropertySet);
        }

        private void OnNamePropertySet()
        {
            if (_applicationUserService.IsValid == false)
            {
                WelcomeText = SettingsNotSavedText;
                IsValid = false;
                return;
            }

            if (DisplayName == _applicationUserService.DisplayName && Name == _applicationUserService.Name)
            {
                WelcomeText = SettingsSavedText;
                IsValid = true;
            }
            else
            {
                WelcomeText = SettingsNotSavedText;
                IsValid = false;
            }        
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand)).
            ObservesProperty(() => WelcomeText);

        private async void ExecuteSaveCommand()
        {
            _applicationUserService.DisplayName = DisplayName;
            _applicationUserService.Name = Name;

            await _applicationUserService.SaveUserDataAsync();

            if (await ShouldNavigateToMainPage)
            {
                await NavigationService.NavigateAsync(MainPage);
                return;
            }
                
            OnNamePropertySet();
        }

        private bool CanExecuteSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            if (string.IsNullOrWhiteSpace(DisplayName))
                return false;

            if (!_applicationUserService.IsValid)
                return true;

            return !(DisplayName == _applicationUserService.DisplayName
                     && Name == _applicationUserService.Name);
        }

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ExecuteClearCommand));

        private async void ExecuteClearCommand()
        {
            await _applicationUserService.ClearAsync();
            InitProperties();
        }

        protected override void ConfigureOnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("FirstLoad"))
            {
                FirstLoad = true;
            }
            InitProperties();
        }

        private void InitProperties()
        {
            Name = _applicationUserService.Name;
            DisplayName = _applicationUserService.DisplayName;
            if(!_applicationUserService.IsValid)
                OnNamePropertySet();
        }

        public bool CanNavigate(INavigationParameters parameters)
        {
            if (!_applicationUserService.IsValid)
            {
                IsValid = _applicationUserService.IsValid;
                WelcomeText = ErrorMsgText;
            }

            return _applicationUserService.IsValid;
        }
    }
}