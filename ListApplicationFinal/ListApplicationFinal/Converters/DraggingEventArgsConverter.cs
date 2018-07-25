using System;
using System.Globalization;
using ListApplicationFinal.Domain;
using ListApplicationFinal.ViewModels;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace ListApplicationFinal.Converters
{
    internal class DraggingCommandArgs : IDraggingCommandArgs
    {
        public DraggingCommandArgs(ItemDraggingEventArgs args)
        {
            NewIndex = args.NewIndex;
            OldIndex = args.OldIndex;
        }

        public int NewIndex { get; }
        public int OldIndex { get; }
    }

    public class DraggingEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ItemDraggingEventArgs dragArgs)
            {
                if (dragArgs.Action == DragAction.Drop &&
                    dragArgs.NewIndex != dragArgs.OldIndex)
                {
                    return new DraggingCommandArgs(dragArgs);
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