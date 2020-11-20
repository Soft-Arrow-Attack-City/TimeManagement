using System;
using System.Globalization;
using System.Windows.Data;

// Cited from https://github.com/Keboo/MaterialDesignInXaml.Examples/blob/master/Dragablz/TabablzControl.FullWidthTabs/DivideValueConverter.cs

namespace TimeManagement
{
    public class DivideValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) / System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}