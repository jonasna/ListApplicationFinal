using System.Linq;
using ListApplicationFinal.ViewModels.Events;
using ListApplicationFinal.ViewModels.Interfaces;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Application = Prism.PrismApplicationBase;

namespace ListApplicationFinal.ViewModels
{
    public class VmBase : BindableBase, INavigationAware, IDestructible
    {
        private static T GetDependency<T>()
        { 
            return Application.Current.Container.Resolve<T>();
        }

        protected INavigationService NavigationService { get; }

        public VmBase(INavigationService navigationService)
        {
            NavigationService = navigationService;

            if (this is IHandlePageNameChange pageHandler)
            {
                SubscribeHandlerToPageNameEvent(pageHandler);
            }
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
            if (this is INotifyPageNameChange pageNotifier)
            {
                if (pageNotifier.ShouldNotify)
                    EmitCurrentPageNameEvent();
            }

           ConfigureOnNavigatedTo(parameters);
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            ConfigureOnNavigatingTo(parameters);
        }

        public void Destroy()
        {
            if (this is IHandlePageNameChange pageHandler)
            {
                UnSubscribeHandlerToPageNameEvent(pageHandler);
            }

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

        #region PageName Event

        private SubscriptionToken _subscriptionToken = null;
        private void EmitCurrentPageNameEvent()
        {
            var currentPage = NavigationService.GetNavigationUriPath().Split('/').Last();
            GetDependency<IEventAggregator>().GetEvent<CurrentPageNameEvent>().Publish(currentPage);
        }
        private void SubscribeHandlerToPageNameEvent(IHandlePageNameChange pageHandler)
        {
            _subscriptionToken = GetDependency<IEventAggregator>().GetEvent<CurrentPageNameEvent>()
                .Subscribe(pageHandler.HandlePageNameChange);
        }
        private void UnSubscribeHandlerToPageNameEvent(IHandlePageNameChange pageHandler)
        {
            GetDependency<IEventAggregator>().GetEvent<CurrentPageNameEvent>()
                .Unsubscribe(_subscriptionToken);
        }

        #endregion
    }
}