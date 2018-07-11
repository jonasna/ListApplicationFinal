using System;
using System.Globalization;
using Xamarin.Forms;

namespace ListApplicationFinal.Converters
{
    public class BoolToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return (Color)Prism.PrismApplicationBase.Current.Resources["OkTextColor"];

            return (Color)Prism.PrismApplicationBase.Current.Resources["NotOkTextColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}