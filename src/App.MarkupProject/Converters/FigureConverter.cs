using MVVM.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.MarkupProject.Models;
using App.MarkupProject.Models.Interfaces;

namespace App.MarkupProject.Converters
{
    public class FigureConverter : ConverterBase<FigureConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IMarkupFigure)
            {
                var fig = (IMarkupFigure)value;
                StringBuilder sb = new StringBuilder();

                foreach (var p in fig.Points)
                {
                    sb.Append(String.Format("{0},{1} ", p.Item1, p.Item2));
                }
                sb.Append(String.Format("{0},{1} ", fig.Points[0].Item1, fig.Points[0].Item2));
                sb.Remove(sb.Length - 1, 1);
                var result = sb.ToString();
                return result;
            }
            return "";
        }
    }
}
