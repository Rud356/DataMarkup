using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using MVVM.Converters;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Controls;

namespace App.MarkupProject.Converters;

public class ListboxIndexConverter : ConverterBase<ListboxIndexConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int)
        {
            var i = (int) value + 1;
            return String.Format("Выделение {0}", i);
        }

        return -1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
