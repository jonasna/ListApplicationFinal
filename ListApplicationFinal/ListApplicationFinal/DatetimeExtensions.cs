using System;

namespace ListApplicationFinal
{
    public static class DatetimeExtensions
    {
        public static string ToTodoFormatString(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM H:mm");
        }
    }
}