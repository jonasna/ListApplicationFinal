using System;
using System.Globalization;
using Xamarin.Forms;

namespace ListApplicationFinal.Converters
{
    public class IsRefreshingToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isRefreshing = (bool) value;
            return !isRefreshing; // Visibility is opposite to refreshing
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = (bool) value;
            return !isVisible;
        }
    }
}