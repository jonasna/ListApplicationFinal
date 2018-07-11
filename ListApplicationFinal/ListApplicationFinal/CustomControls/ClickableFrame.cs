using System;
using Xamarin.Forms;

namespace ListApplicationFinal.CustomControls
{
    public class ClickableFrame : Frame, IButtonController
    {
        public event EventHandler Clicked;
        public event EventHandler Pressed;
        public event EventHandler Released;

        public virtual void SendClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        public virtual void SendPressed()
        {
            Pressed?.Invoke(this, EventArgs.Empty);
        }

        public virtual void SendReleased()
        {
            Released?.Invoke(this, EventArgs.Empty);
        }
    }
}