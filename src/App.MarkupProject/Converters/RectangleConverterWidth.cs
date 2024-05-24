using App.MarkupProject.Models;
using MVVM.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace App.MarkupProject.Converters
{
    public class RectangleConverterWidth : ConverterBase<RectangleConverterWidth>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Rectangle)
            {
                Rectangle r = (Rectangle)value;
                var topLeft = r.Points[0];
                var bottomRight = r.Points[2];

                var Width = bottomRight.Item1 - topLeft.Item1;
                var Height = bottomRight.Item2 - topLeft.Item2;

                return Width;
            }
            return 0;
        }
    }

    public class RectangleConverterHeight : ConverterBase<RectangleConverterHeight>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Rectangle)
            {
                Rectangle r = (Rectangle)value;
                var topLeft = r.Points[0];
                var bottomRight = r.Points[2];

                var Width = bottomRight.Item1 - topLeft.Item1;
                var Height = bottomRight.Item2 - topLeft.Item2;

                return Height;
            }
            return 0;
        }
    }

    public class RectangleConverterTop : ConverterBase<RectangleConverterTop>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Rectangle)
            {
                Rectangle r = (Rectangle)value;
                var topLeft = r.Points[0];
                var bottomRight = r.Points[2];

                var Width = bottomRight.Item1 - topLeft.Item1;
                var Height = bottomRight.Item2 - topLeft.Item2;

                return topLeft.Item1;
            }
            return 0;
        }
    }

    public class RectangleConverterLeft : ConverterBase<RectangleConverterLeft>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Rectangle)
            {
                Rectangle r = (Rectangle)value;
                var topLeft = r.Points[0];
                var bottomRight = r.Points[2];

                var Width = bottomRight.Item1 - topLeft.Item1;
                var Height = bottomRight.Item2 - topLeft.Item2;

                return topLeft.Item2;
            }
            return 0;
        }
    }
}
