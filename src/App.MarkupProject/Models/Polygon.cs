using App.MarkupProject.Models.Interfaces;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace App.MarkupProject.Models
{
    /// <summary>
    /// Points are given in (x, y) format, with ints repserenting pixels poisiton on image.
    /// </summary>
    public class Polygon : ReactiveObject, IMarkupFigure
    {
        private ObservableCollection<Tuple<int, int>> _points;
        private string _className;
        private bool _isHidden;

        public Polygon()
        {
            _points = new();
            _className = string.Empty;
            _isHidden = false;
        }

        public Polygon(string className) : this()
        {
            _className = className;
        }

        public Polygon(string className, bool isHidden) : this(className)
        {
            _isHidden = isHidden;
        }

        [Reactive] public string AssignedClassName { get => _className; set => _className = value; }
        [Reactive] public bool IsHidden { get => _isHidden; set => _isHidden = value; }

        [Reactive] public ObservableCollection<Tuple<int, int>> Points => _points;
    }
}
