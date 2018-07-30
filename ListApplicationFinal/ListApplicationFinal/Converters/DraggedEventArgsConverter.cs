using System;
using System.Globalization;
using System.Threading.Tasks;
using ListApplicationFinal.Domain;
using ListApplicationFinal.ViewModels;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace ListApplicationFinal.Converters
{
    internal class DraggedCommandArgs : IDraggedCommandArgs
    {
        public DraggedCommandArgs(ItemDraggingEventArgs args)
        {
            if(args == null) throw new ArgumentNullException(nameof(args));
            NewIndex = args.NewIndex;
            OldIndex = args.OldIndex;
            IsValid = (args.Action == DragAction.Drop);
        }

        public int NewIndex { get; }
        public int OldIndex { get; }
        public bool IsValid { get; set; }
    }

    public class DraggedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dragEventArgs = (ItemDraggingEventArgs)value;
            return new DraggedCommandArgs(dragEventArgs);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}