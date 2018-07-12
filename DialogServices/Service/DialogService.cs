using System;
using System.Linq;
using System.Threading.Tasks;
using Prism;
using Prism.Navigation;
using Prism.Unity;
using NavigationService = Prism.Plugin.Popups.PopupPageNavigationService;

namespace DialogServices.Service
{
    public class DialogService : IDialogService
    {
        protected readonly INavigationService Navigation;

        public DialogService(INavigationService navigationService)
        {
            Navigation = navigationService;
        }

        public async Task<bool> QuestionDialog(string question, string title, string okBtnText = "Ok", string notOkBtnText = "Cancel")
        {
            var navParams = new NavigationParameters()
            {
                {"Question", question},
                {"Title", title},
                {"OkBtnText", okBtnText},
                {"NotOkBtnText", notOkBtnText}
            };

            return await Navigate<bool>(navParams, "QuestionDialogPage");
        }

        private async Task<T> Navigate<T>(INavigationParameters navParams, string uri)
        {
            var source = new TaskCompletionSource<T>();
            navParams.Add("Source", source);

            await Navigation.NavigateAsync(new Uri(uri, UriKind.Relative), navParams);
            return await source.Task;
        }
    }


}