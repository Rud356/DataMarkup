using App.MarkupProject.Models.Interfaces;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Configuration;

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
        private ObservableCollection<string> _labels;

        public Polygon(ref ObservableCollection<string> labels)
        {
            _points = new();
            AssignedClassID = -1;
            _isHidden = false;
            _labels = labels;
        }

        public Polygon(ref ObservableCollection<string> labels, int classID) : this(ref labels)
        {
            AssignedClassID = classID;
        }

        public Polygon(ref ObservableCollection<string> labels, int classID, bool isHidden) : this(ref labels, classID)
        {
            _isHidden = isHidden;
        }

        private string _label = "Not assigned";

        [Reactive] public string AssignedClass {
            get {
                return _label;
            }
            set
            {
                _classID = _labels.IndexOf(value);
                _label = _labels[_classID];
            }
        }

        [Reactive] public int AssignedClassID { get => _classID; set => _classID = value; }
        [Reactive] public bool IsHidden { get => _isHidden; set => _isHidden = value; }

        [Reactive] public ObservableCollection<Tuple<int, int>> Points => _points;

        public ref ObservableCollection<string> Labels { get => ref _labels; }
    }
}
