using System;
using Windows.UI.Xaml.Data;

namespace PokeDex.controls
{
    public class ConvertPercent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return System.Convert.ToDouble(value) *
               (System.Convert.ToDouble(parameter) / 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
