using MVVM.Converters;
using System.Windows;

namespace App.MarkupProject.Converters
{
    public class PolysVisibilityConverter : BooleanConverters<Visibility, PolysVisibilityConverter>
    {
        public PolysVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
        {

        }
    }
    
}
