using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Resync_Edit.Foundation
{
    // Taken from https://github.com/unosquare/ffmediaelement/blob/bae88aacc70d29a51e2d7cf59c68628955754b54/Unosquare.FFME.Windows.Sample/Foundation/ValueConverters.cs
    public class TimeSpanToSecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case TimeSpan span:
                    return span.TotalSeconds;
                case Duration duration:
                    return duration.HasTimeSpan ? duration.TimeSpan.TotalSeconds : 0d;
                default:
                    return 0d;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if ((value is double) == false) return 0d;
            var result = TimeSpan.FromTicks(System.Convert.ToInt64(TimeSpan.TicksPerSecond * (double)value));

            if (targetType == typeof(TimeSpan)) return result;
            return targetType == typeof(Duration) ? new Duration(result) : Activator.CreateInstance(targetType);
        }
    }
}
