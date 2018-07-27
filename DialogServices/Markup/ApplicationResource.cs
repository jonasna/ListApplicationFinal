using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DialogServices.Markup
{
    public class ApplicationResource : IMarkupExtension
    {
        public string ResourcePath { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if(ResourcePath == null) throw new ArgumentNullException(nameof(ResourcePath));
            return Application.Current.Resources[ResourcePath];
        }
    }
}