using Xamarin.Forms;

namespace DialogServices.Register
{
    public enum DialogSetting
    {
        AcceptButtonColor,
        CancelButtonColor,
        QuestionDialogBackgroundColor
    }

    public interface IDialogConfigurationBuilder
    {
        void AddSetting(DialogSetting setting, string resource);
        bool ContainsSetting(DialogSetting setting);
        string GetResourcePath(DialogSetting setting);
        string this[DialogSetting setting] { get; set; }
    }
}