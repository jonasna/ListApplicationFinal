using System.Threading.Tasks;
using DialogServices.Service;
using ListApplicationFinal.DataServices;
using Prism.Commands;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class LoginPageViewModel : VmBase, IConfirmNavigation
    {
        private const string SettingsSavedText = "Your settings have been saved";
        private const string SettingsNotSavedText = "Your settings have not been saved";
        private const string SettingsClearedText = "Fill out the form before continuing";

        private const string DialogQuestion =
            "Your settings have been saved succesfully. Would you like to navigate to main page?";

        private const string MainPage = "../ListsOverviewPage";

        private readonly IApplicationUserService _applicationUserService;
        private readonly IDialogService _dialogService;

        public LoginPageViewModel(INavigationService navigationService,
            IApplicationUserService applicationUserService,
            IDialogService dialogService) 
            : base(navigationService)
        {
            Title = "Login Page";

            _applicationUserService = applicationUserService;
            _dialogService = dialogService;
        }

        private Task<bool> ShouldNavigateToMainPage =>
            _dialogService.QuestionDialog(DialogQuestion, "Question", "Yes", "No");

        private bool _firstLoad;
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
                if (!SetProperty(ref _welcomeText, value) && !DoSettingsMatch)
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
            get => _name ?? string.Empty;
            set => SetProperty(ref _name, value ?? string.Empty, OnNamePropertySet);
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName ?? string.Empty;
            set => SetProperty(ref _displayName, value ?? string.Empty, OnNamePropertySet);
        }

        private bool DoSettingsMatch => DisplayName.Trim() == _applicationUserService.DisplayName &&
                                        Name.Trim() == _applicationUserService.Name;

        private void OnNamePropertySet()
        {

            if (!_applicationUserService.IsValid)
            {
                WelcomeText = SettingsClearedText;
                IsValid = false;
                return;
            }

            if (DoSettingsMatch)
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

        private void InitProperties()
        {
            Name = _applicationUserService.Name;
            DisplayName = _applicationUserService.DisplayName;
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand)).
            ObservesProperty(() => WelcomeText);

        private async void ExecuteSaveCommand()
        {
            _applicationUserService.DisplayName = DisplayName.Trim();
            _applicationUserService.Name = Name.Trim();

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

            return !DoSettingsMatch;
        }

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ExecuteClearCommand));

        private async void ExecuteClearCommand()
        {
            await _applicationUserService.ClearAsync();
            InitProperties();
        }

        protected override void ConfigureOnNavigatedTo(INavigationParameters parameters) => InitProperties();

        protected override void ConfigureOnNavigatingTo(INavigationParameters parameters)
        {
            FirstLoad = parameters.ContainsKey("FirstLoad");
        }

        public bool CanNavigate(INavigationParameters parameters) => _applicationUserService.IsValid;

    }
}