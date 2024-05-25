using MVVM.Converters;
using ReactiveUI;
using System.Globalization;
using System.Windows.Media;

namespace App.MarkupProject.Converters;

internal class Brushes
{
    public static SolidColorBrush PolyBrush = new SolidColorBrush(Colors.Coral);
    public static SolidColorBrush RectangleBrush = new SolidColorBrush(Colors.PaleTurquoise);
    public static SolidColorBrush GenericBrush = new SolidColorBrush(Colors.PowderBlue);
    public static SolidColorBrush SelectedBrush = new SolidColorBrush(Colors.IndianRed);
}

public class PolyColorConverter : ConverterBase<PolyColorConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Models.Rectangle)
        {
            var rectangle = (Models.Rectangle)value;
            if (rectangle.IsSelected || rectangle.Points.Any(x => x.IsSelected))
            {
                return Brushes.SelectedBrush;
            }
            return Brushes.RectangleBrush;
        }

        else if (value is Models.Polygon)
        {
            var poly = (Models.Polygon) value;
            if (poly.IsSelected || poly.Points.Any(x => x.IsSelected))
            {
                return Brushes.SelectedBrush;
            }
            return Brushes.PolyBrush;
        }
        else
        {
            return Brushes.GenericBrush;
        }
            
    }
}
