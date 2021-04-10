using ScreenAnnotation.Resources;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ScreenAnnotation.EditorPage
{
    internal class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string name)
                return string.IsNullOrWhiteSpace(name) ? null : ImageSource.FromResource($"{ImageResourceExtension.resource}{name}.png");
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
