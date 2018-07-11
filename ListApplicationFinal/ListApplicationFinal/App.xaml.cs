using System;
using ListApplicationFinal.DataServices;
using ListApplicationFinal.Pages;
using ListApplicationFinal.ViewModels;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace ListApplicationFinal
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            if(!ApplicationUserService.IsValid)
            {
                await NavigationService.NavigateAsync("LoginPage");
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/MainPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IApplicationUserService, ApplicationUserService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }


    }
}
