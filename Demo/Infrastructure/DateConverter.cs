using System;
using System.Windows.Data;
using System.Globalization;
using System.Diagnostics;

namespace Demo.Infrastructure
{
    [ValueConversion(typeof(DateTime), typeof(String))]
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            String dateString = date.ToString("MM/dd/yy");
            if (dateString == "01/01/01")
            {
                dateString = "";
            }
            return dateString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Reverse DateTime conversion is not implemented");
        }
    }
}
