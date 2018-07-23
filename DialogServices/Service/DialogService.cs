using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace DialogServices.Service
{
    public class DialogService : IDialogService
    {
        public async Task<bool> QuestionDialog(string question, string title, string okBtnText = "Ok", string notOkBtnText = "Cancel")
        {
            var questionPopup = new QuestionPopup
            {
                Question = question,
                Title = title,
                OkBtnText = okBtnText,
                NotOkBtnText = notOkBtnText
            };

            return await Navigate(questionPopup);
        }

        public async Task<string> StringQueryDialog(string queryQuestion)
        {
            var queryStringDialog = new QueryStringPopup
            {
                QueryQuestion = queryQuestion
            };

            return await Navigate(queryStringDialog);
        }

        private static async Task<T> Navigate<T>(PopupBase<T> popup)
        {           
            popup.Completion = new TaskCompletionSource<T>();
            popup.ResultSetEvent += PopupOnOnResultSetEvent;
            
            await PopupNavigation.Instance.PushAsync(popup);
            return await popup.Completion.Task;
        }

        private static async void PopupOnOnResultSetEvent(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.RemovePageAsync((PopupPage) sender);
            var popup = (INotifyResultSet) sender;

            popup.ResultSetEvent -= PopupOnOnResultSetEvent;
        }
    }


}