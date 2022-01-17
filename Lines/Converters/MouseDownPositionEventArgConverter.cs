using Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Lines.Converters
{
    public class MouseDownPositionEventArgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var grid = parameter as Grid;
            var mouseEventArgs = value as MouseButtonEventArgs;
            var position = mouseEventArgs.GetPosition(grid);
            return new Point(System.Convert.ToInt32( position.X), System.Convert.ToInt32(position.Y));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
