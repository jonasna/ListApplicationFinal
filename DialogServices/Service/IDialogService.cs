using System.Threading.Tasks;

namespace DialogServices.Service
{
    public interface IDialogService
    {
        Task<bool> QuestionDialog(string question, string title, string okBtnText = "Ok", string notOkBtnText = "Cancel");
        Task<string> StringQueryDialog(string queryQuestion);
    }
}