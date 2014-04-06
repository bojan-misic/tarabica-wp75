using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Tarabica.WP7App.Infrastructure.Converters
{

    public class StringToCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = Int32.Parse((string)parameter);
            var text = (string)value;
            if (index >= text.Length)
                return ' ';
            return text[index];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
