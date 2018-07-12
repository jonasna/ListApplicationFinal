using System.Linq;
using ListApplicationFinal.ViewModels.Events;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;

namespace ListApplicationFinal.ViewModels
{
    public class VmBase : BindableBase, INavigationAware, IDestructible
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected INavigationService NavigationService { get; }
        protected IEventAggregator EventAggregator { get; }

        public VmBase(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            NavigationService = navigationService;
            EventAggregator = eventAggregator;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
           
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }

        protected void EmitCurrentPageNameEvent()
        {
            var currentPage = NavigationService.GetNavigationUriPath().Split(new[] { '/' }).Last();
            EventAggregator.GetEvent<CurrentPageNameEvent>().Publish(currentPage);
        }
    }
}