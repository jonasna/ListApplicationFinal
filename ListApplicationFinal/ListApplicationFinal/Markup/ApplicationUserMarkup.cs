using System;
using System.Collections.Generic;
using System.Text;
using ListApplicationFinal.DataServices;
using Prism;
using Prism.Ioc;
using Xamarin.Forms.Xaml;

namespace ListApplicationFinal.Markup
{
    public enum UserData
    {
        Name,
        DisplayName
    }

    public class ApplicationUserMarkup : IMarkupExtension
    {
        public UserData Data { get; set; }
        public string StringFormat { get; set; } = null;

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                if (StringFormat != null)
                {
                    return string.Format(StringFormat, GetDataString(Data));
                }
                else
                {
                    return GetDataString(Data);
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        private static string GetDataString(UserData data)
        {
            var userService = DependencyExtensions.GetDependency<IApplicationUserService>();
            if (data == UserData.Name)
                return userService.Name;
            return userService.DisplayName;
        }
    }
}
