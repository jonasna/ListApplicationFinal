using System;
using System.Linq;
using Debug = System.Diagnostics.Debug;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Platform;
using ListApplicationFinal.Droid.CustomRenderers;
using ListApplicationFinal.Pages;
using Prism;
using Prism.Ioc;

namespace ListApplicationFinal.Droid
{
    [Activity(Label = "List App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Rg.Plugins.Popup.Popup.Init(this, bundle);
            
            // License here

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            CachedImageRenderer.Init(true);
            ClickableFrameRenderer.Init();


            try
            {
                LoadApplication(new App(new AndroidInitializer()));
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {

            }
            else
            {

            }
        }

    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            // Register any platform specific implementations

        }
    }

}

