using Prism.Mvvm;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class VmBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; }

        public VmBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            ConfigureOnNavigatedFrom(parameters);
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
           ConfigureOnNavigatedTo(parameters);
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            ConfigureOnNavigatingTo(parameters);
        }

        public void Destroy()
        {
            ConfigureDestroy();
        }

        protected virtual void ConfigureOnNavigatedTo(INavigationParameters parameters)
        {}

        protected virtual void ConfigureOnNavigatedFrom(INavigationParameters parameters)
        {}

        protected virtual void ConfigureOnNavigatingTo(INavigationParameters parameters)
        {}

        protected virtual void ConfigureDestroy()
        {

        }
    }
}