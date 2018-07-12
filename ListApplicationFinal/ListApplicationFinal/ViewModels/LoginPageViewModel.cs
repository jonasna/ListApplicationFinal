using System;
using System.Collections.ObjectModel;
using ListApplicationFinal.DataServices;
using ListApplicationFinal.ViewModels.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class LoginPageViewModel : VmBase, IConfirmNavigation
    {
        private const string SettingsSavedText = "Your settings have been saved";
        private const string SettingsNotSavedText = "Your settings have not been saved";
        private const string ErrorMsgText = "Fill out the form before continuing";

        private readonly IApplicationUserService _applicationUserService;

        public LoginPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, IApplicationUserService applicationUserService) 
            : base(navigationService, eventAggregator)
        {
            Title = "Login Page";
            _applicationUserService = applicationUserService;           
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
            set => SetProperty(ref _welcomeText, value);
        }

        private bool _isValid;    
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand)).
            ObservesProperty(() => Name).ObservesProperty(() => DisplayName).ObservesProperty(() => IsValid);

        private async void ExecuteSaveCommand()
        {
            _applicationUserService.DisplayName = DisplayName;
            _applicationUserService.Name = Name;
            await _applicationUserService.SaveUserDataAsync();

            if (_firstLoad)
                await NavigationService.NavigateAsync("/MasterPage/NavBarPage/MainPage");
            else
                UpdateApplicationUserProperties();

        }

        private bool CanExecuteSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            if (string.IsNullOrWhiteSpace(DisplayName))
                return false;

            if (DisplayName == _applicationUserService.DisplayName)
                return false;

            if (Name == _applicationUserService.Name)
                return false;

            return true;
        }

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ExecuteClearCommand));

        private async void ExecuteClearCommand()
        {
            await _applicationUserService.ClearAsync();
            UpdateApplicationUserProperties();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("FirstLoad"))
            {
                FirstLoad = true;
            }

            EmitCurrentPageNameEvent();
            UpdateApplicationUserProperties();
        }

        private void UpdateApplicationUserProperties()
        {
            IsValid = _applicationUserService.IsValid;
            WelcomeText = IsValid ? SettingsSavedText : SettingsNotSavedText;
            Name = _applicationUserService.Name;
            DisplayName = _applicationUserService.DisplayName;
        }

        public bool CanNavigate(INavigationParameters parameters)
        {
            if (!_applicationUserService.IsValid)
            {
                WelcomeText = ErrorMsgText;
            }

            return _applicationUserService.IsValid;
        }
    }
}