using Android.App;
using Android.Widget;
using ListApplicationFinal.Toast;

namespace ListApplicationFinal.Droid.Toast
{
    public class ToastProvider : IToastProvider
    {
        public void LongMessage(string message)
        {
            Android.Widget.Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortMessage(string message)
        {
            Android.Widget.Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}