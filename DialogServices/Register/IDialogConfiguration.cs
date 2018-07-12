using Xamarin.Forms;

namespace DialogServices.Register
{
    public interface IDialogConfiguration
    {
        Color AcceptButtonColor { get; }
        Color CancelButtonColor { get; }
        Color QuestionDialogBackgroundColor { get; }

        int ButtonTextSize { get; }
        int DialogTextSize { get; }
        int Padding { get; }
    }
}