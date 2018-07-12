using System;
using System.Collections.ObjectModel;
using DialogServices.Service;
using ListApplicationFinal.DataServices;
using ListApplicationFinal.ViewModels.Events;
using ListApplicationFinal.ViewModels.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class LoginPageViewModel : VmBase, IConfirmNavigation, INotifyPageNameChange
    {
        private const string SettingsSavedText = "Your settings have been saved";
        private const string SettingsNotSavedText = "Your settings have not been saved";
        private const string ErrorMsgText = "Fill out the form before continuing";

        private readonly IApplicationUserService _applicationUserService;
        private readonly IDialogService _dialogService;
        public bool ShouldNotify { get; set; } // INotifyPageName

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
            ObservesProperty(() => Name).ObservesProperty(() => DisplayName);

        private async void ExecuteSaveCommand()
        {
            _applicationUserService.DisplayName = DisplayName;
            _applicationUserService.Name = Name;
            await _applicationUserService.SaveUserDataAsync();

            if (FirstLoad)
                await NavigationService.NavigateAsync("/MasterPage/NavBarPage/MainPage");
            else
            {
                if (await _dialogService.QuestionDialog(
                    "Your settings have been saved succesfully. Would you like to navigate to main page?", "Question", "Yes", "No"))
                    await NavigationService.NavigateAsync("/MasterPage/NavBarPage/MainPage");
                else
                {
                    UpdateApplicationUserProperties();
                }
            }
        }

        private bool CanExecuteSaveCommand()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            if (string.IsNullOrWhiteSpace(DisplayName))
                return false;

            if (DisplayName == _applicationUserService.DisplayName && Name == _applicationUserService.Name)
            {
                WelcomeText = SettingsSavedText;
                IsValid = true;
                return false;
            }
            else
            {
                WelcomeText = SettingsNotSavedText;
                IsValid = false;
                return true;
            }

        }

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ExecuteClearCommand));

        private async void ExecuteClearCommand()
        {
            await _applicationUserService.ClearAsync();
            UpdateApplicationUserProperties();
        }

        protected override void ConfigureOnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("FirstLoad"))
            {
                FirstLoad = true;
            }

            UpdateApplicationUserProperties();
        }

        public bool CanNavigate(INavigationParameters parameters)
        {
            if (!_applicationUserService.IsValid)
            {
                WelcomeText = ErrorMsgText;
                IsValid = _applicationUserService.IsValid;
            }

            return _applicationUserService.IsValid;
        }

        private void UpdateApplicationUserProperties()
        {
            IsValid = _applicationUserService.IsValid;
            WelcomeText = IsValid ? SettingsSavedText : SettingsNotSavedText;
            Name = _applicationUserService.Name;
            DisplayName = _applicationUserService.DisplayName;

            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}