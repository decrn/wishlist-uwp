using System;
using Windows.UI.Xaml.Data;

// Source: https://stackoverflow.com/a/41962421
namespace ClientApp.Helpers {
    public class TimeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return new DateTimeOffset(((DateTime)value).ToUniversalTime());

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return ((DateTimeOffset)value).DateTime;
        }
    }
}
