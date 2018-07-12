using DialogServices.Pages;
using DialogServices.Service;
using DialogServices.ViewModels;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Prism.Unity;
using Unity.Lifetime;

namespace DialogServices.Register
{
    public static class RegisterDialogServicesExtention
    {
        public static void RegisterDialogServices(this IContainerRegistry containerRegistry, IDialogConfiguration configuration = null)
        {

            configuration = configuration ?? new DialogConfiguration();

            containerRegistry.RegisterPopupNavigationService();

            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.GetContainer().RegisterInstance(typeof(IDialogConfiguration), "iDialogConfiguration", configuration,
                    new ContainerControlledLifetimeManager());

            containerRegistry.RegisterForNavigation<QuestionDialogPage, QuestionDialogPageViewModel>();
        }
    }
}