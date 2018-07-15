using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using Xamarin.Forms.Xaml;
using ListApplicationFinal.DataServices;
using ListApplicationFinal.Pages;
using ListApplicationFinal.ViewModels;
using DialogServices.Register;
using Xamarin.Forms;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace ListApplicationFinal
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        private IContainerRegistry _containerRegistry;

        protected override async void OnInitialized()
        {
            InitializeComponent();

            if (!ApplicationUserService.IsValid)
            {
                var firstLoadParams = new NavigationParameters {{"FirstLoad", null}};
                await NavigationService.NavigateAsync("/LoginPage", firstLoadParams);
            }
            else
            {
                await NavigationService.NavigateAsync("/MasterPage/NavBarPage/MainPage");
            }

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _containerRegistry = containerRegistry;

            containerRegistry.RegisterSingleton<IApplicationUserService, ApplicationUserService>();
            containerRegistry.RegisterSingleton<ITodoService, TodoService>();

            containerRegistry.RegisterForNavigation<NavBarPage, NavBarPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ListsOverviewPage, ListsOverviewPageViewModel>();
            containerRegistry.RegisterForNavigation<MasterPage, MasterPageViewModel>(); // Master detail

            var dialogConfig = new DialogConfigurationBuilder
            {
                [DialogSetting.QuestionDialogBackgroundColor] = "InfoModalBackgroundColor",
                [DialogSetting.AcceptButtonColor] = "OkTextColor",
                [DialogSetting.CancelButtonColor] = "NotOkTextColor"
            };

            _containerRegistry.RegisterDialogServices(dialogConfig);
        }


    }
}
