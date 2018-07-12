using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace DialogServices
{
    public class PopupPageBase : PopupPage
    {
        public PopupPageBase()
        {

        }

        protected override async void OnAppearingAnimationEnd()
        {
            await Content.FadeTo(1);
        }

        // Method for animation child in PopupPage
        // Invoked before custom animation begin
        protected override async void OnDisappearingAnimationBegin()
        {
            await Content.FadeTo(1);
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent back button pressed action on android
            return true;
        }

        // Invoced when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Prevent background clicked action
            return false;
        }

    }
}