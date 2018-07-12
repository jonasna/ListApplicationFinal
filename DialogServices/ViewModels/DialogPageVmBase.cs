using System.Threading.Tasks;
using Prism;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;

namespace DialogServices.ViewModels
{
    public class DialogPageVmBase<T> : BindableBase, INavigationAware
    {
        private TaskCompletionSource<T> CompletionSource { get; set; } = null;

        protected INavigationService Navigation { get; }

        public DialogPageVmBase(INavigationService navigationService)
        {
            Navigation = navigationService;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Source"))
            {
                CompletionSource = (TaskCompletionSource<T>) parameters["Source"];
                ConfigureOnLoad(parameters);
            }
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        protected virtual void ConfigureOnLoad(INavigationParameters parameters)
        {

        }

        protected async void SetCompleted(T completedResult)
        {
            CompletionSource.SetResult(completedResult);
            await Navigation.ClearPopupStackAsync();
        }
    }
}