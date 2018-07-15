using Prism;
using Prism.Ioc;

namespace ListApplicationFinal
{
    public static class DependencyExtensions
    {
        public static T GetDependency<T>() => PrismApplicationBase.Current.Container.Resolve<T>();
    }
}