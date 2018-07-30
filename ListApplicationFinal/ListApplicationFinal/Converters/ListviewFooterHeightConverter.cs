using System;
using System.Globalization;
using Syncfusion.ListView.XForms;
using Syncfusion.ListView.XForms.Control.Helpers;
using Xamarin.Forms;

namespace ListApplicationFinal.Converters
{
    public class ListviewFooterHeightConverter : IValueConverter
    {
        private const int DefaultFooterSize = 90;
        private static double FullSize => Application.Current.MainPage.Height;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = (SfListView) parameter;
            var isLoading = (bool) value;

            double height;

            if (isLoading)
            {
                height = FullSize;
            }
            else
            {
                height = DefaultFooterSize;
            }

            return height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}