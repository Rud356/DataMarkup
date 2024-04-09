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
        private int _classID;
        private bool _isHidden;

        public Polygon()
        {
            _points = new();
            _classID = -1;
            _isHidden = false;
        }

        public Polygon(int classID) : this()
        {
            _classID = classID;
        }

        public Polygon(int classID, bool isHidden) : this(classID)
        {
            _isHidden = isHidden;
        }

        [Reactive] public int AssignedClassID { get => _classID; set => _classID = value; }
        [Reactive] public bool IsHidden { get => _isHidden; set => _isHidden = value; }

        [Reactive] public ObservableCollection<Tuple<int, int>> Points => _points;
    }
}
