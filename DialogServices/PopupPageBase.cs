using System.Windows.Input;
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

            BackBtnPressedCommand?.Execute(null);

            return true;
        }

        // Invoced when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Prevent background clicked action

            BackGroundPressedCommand?.Execute(null);

            return false;
        }

        public static BindableProperty BackBtnPressedCommandProperty = BindableProperty.Create(nameof(BackBtnPressedCommand), typeof(ICommand), typeof(PopupPageBase), null);
        public static BindableProperty BackGroundPressedCommandProperty = BindableProperty.Create(nameof(BackGroundPressedCommand), typeof(ICommand), typeof(PopupPageBase), null);

        public ICommand BackBtnPressedCommand
        {
            set => SetValue(BackBtnPressedCommandProperty, value);
            get => (ICommand) GetValue(BackBtnPressedCommandProperty);
        }

        public ICommand BackGroundPressedCommand
        {
            set => SetValue(BackGroundPressedCommandProperty, value);
            get => (ICommand)GetValue(BackGroundPressedCommandProperty);
        }

    }
}