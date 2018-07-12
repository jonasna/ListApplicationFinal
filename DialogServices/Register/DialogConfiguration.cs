using Xamarin.Forms;

namespace DialogServices.Register
{
    public class DialogConfiguration : IDialogConfiguration
    {
        public Color AcceptButtonColor { get; set; } = Color.LawnGreen;
        public Color CancelButtonColor { get; set; } = Color.Red;
        public Color QuestionDialogBackgroundColor { get; set; } = Color.White;
        public int ButtonTextSize { get; set; } = 16;
        public int DialogTextSize { get; set; } = 16;
        public int Padding { get; set; } = 5;
    }
}