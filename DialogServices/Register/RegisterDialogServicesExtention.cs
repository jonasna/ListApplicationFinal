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
        public static void RegisterDialogServices(this IContainerRegistry containerRegistry, IDialogConfigurationBuilder configuration = null)
        {

            configuration = configuration ?? new DialogConfigurationBuilder();

            containerRegistry.RegisterPopupNavigationService();

            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.GetContainer().RegisterInstance(typeof(IDialogConfigurationBuilder), "iDialogConfiguration", configuration,
                    new ContainerControlledLifetimeManager());

            containerRegistry.RegisterForNavigation<QuestionDialogPage, QuestionDialogPageViewModel>();
        }
    }
}