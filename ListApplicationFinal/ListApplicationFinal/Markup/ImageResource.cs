using System;
using System.Reflection;
using FFImageLoading.Svg.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListApplicationFinal.Markup
{
    [ContentProperty("Source")]
    public class ImageResource : IMarkupExtension
    {
        public string Source { get; set; }
        private readonly string _base;

        public ImageResource()
        {
            _base = Assembly.GetCallingAssembly().GetName().Name + ".Images.";
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
                return null;

            return SvgImageSource.FromResource(_base + Source);
        }
    }
}