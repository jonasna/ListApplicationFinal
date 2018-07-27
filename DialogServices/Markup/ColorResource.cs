using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DialogServices.Markup
{
    public class ColorResource : IMarkupExtension<Color>
    {
        public string ResourcePath { get; set; }

        public Color ProvideValue(IServiceProvider serviceProvider)
        {
            if (ResourcePath == null) throw new ArgumentNullException(nameof(ResourcePath));
            return (Color)Application.Current.Resources[ResourcePath];
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }
}