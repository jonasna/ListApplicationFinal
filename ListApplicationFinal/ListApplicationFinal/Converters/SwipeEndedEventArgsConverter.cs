using System;
using System.Globalization;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace ListApplicationFinal.Converters
{
    public class SwipeEndedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SwipeEndedEventArgs swipeArgs)
            {
                if (swipeArgs.SwipeOffset > 0) // If offset is different from zero the swipe threshhold has been reached
                {
                    return swipeArgs.ItemIndex;                    
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}