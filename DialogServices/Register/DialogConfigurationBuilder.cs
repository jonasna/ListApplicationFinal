using System.Collections.Generic;
using Xamarin.Forms;

namespace DialogServices.Register
{
    public class DialogConfigurationBuilder : IDialogConfigurationBuilder
    {
        private readonly Dictionary<DialogSetting, string> _resourceKeys
            = new Dictionary<DialogSetting, string>();

        public void AddSetting(DialogSetting setting, string resource)
        {
            _resourceKeys[setting] = resource;
        }

        public bool ContainsSetting(DialogSetting setting)
        {
            return _resourceKeys.ContainsKey(setting);
        }

        public string GetResourcePath(DialogSetting setting)
        {
            if (_resourceKeys.TryGetValue(setting, out var value))
                return value;

            return null;
        }

        public string this[DialogSetting setting]
        {
            get => _resourceKeys.ContainsKey(setting) ? _resourceKeys[setting] : null;
            set => _resourceKeys[setting] = value;
        }
    }

    public static class DialogConfigurationExtensions
    {
        public static bool GetResource<T>(this IDialogConfigurationBuilder builder, DialogSetting target, out T result)
        {
            if (builder.ContainsSetting(target))
            {
                result = (T)Application.Current.Resources[builder[target]];
                return true;
            }

            result = default(T);
            return false;
        }
    }
}