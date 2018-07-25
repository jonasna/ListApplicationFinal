using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using Xamarin.Forms.Xaml;
using ListApplicationFinal.DataServices;
using ListApplicationFinal.Pages;
using ListApplicationFinal.ViewModels;
using DialogServices.Register;
using DialogServices.Service;
using ListApplicationFinal.Domain;
using Prism.Mvvm;
using Xamarin.Forms;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace ListApplicationFinal
{
    public partial class App : PrismApplication
    {
        private const string LoginPageUri = "/MasterPage/NavBarPage/LoginPage";
        private const string MainPageUri = "/MasterPage/NavBarPage/ListsOverviewPage";
        
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            if (!ApplicationUserService.IsValid)
            {
                var firstLoadParams = new NavigationParameters {{"FirstLoad", null}};
                await NavigationService.NavigateAsync(LoginPageUri, firstLoadParams);
            }
            else
            {
                await NavigationService.NavigateAsync(MainPageUri);
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IApplicationUserService, ApplicationUserService>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterSingleton<ITodoStore, FakeTodoStore>();

            containerRegistry.RegisterForNavigation<NavBarPage, NavBarPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ListsOverviewPage, ListsOverviewPageViewModel>();
            containerRegistry.RegisterForNavigation<MasterPage, MasterPageViewModel>(); // Master detail
            containerRegistry.RegisterForNavigation<SingleListPage, SingleListPageViewModel>();
        }
    }
}
