using App.MarkupProject.Models.Interfaces;
using DynamicData.Tests;
using Microsoft.VisualBasic;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace App.MarkupProject.Models
{
    /// <summary>
    /// Points go from top left corner to bottom left corner in clockwise order.
    /// Points are given in (x, y) format, with ints repserenting pixels poisiton on image.
    /// </summary>
    public class Rectangle : IMarkupFigure
    {
        private ObservableCollection<Vertex> points; 
        private Vertex _topCorner, _bottomCorner;
        private int _classID;
        private bool _isVisible;
        private ObservableCollection<string> _labels;

        private string _label = "Not assigned";

        [Reactive]
        public string AssignedClass
        {
            get
            {
                return _label;
            }
            set
            {
                _classID = _labels.IndexOf(value);
                _label = _labels[_classID];
            }
        }

        [Reactive] public int AssignedClassID { get => _classID; set => _classID = value; }
        [Reactive] public bool IsVisible { get => _isVisible; set => _isVisible = value; }

        [Reactive] public ObservableCollection<Vertex> Points => points;

        public ref ObservableCollection<string> Labels { get => ref _labels; }

        public Rectangle(ref ObservableCollection<string> labels, Tuple<int, int, int, int> bbox, int classID)
        {
            AssignedClassID = classID;
            IsVisible = true;
            _labels = labels;

            var _ = new Vertex(0, 0);
            points = new ObservableCollection<Vertex>();
            points.Add(_);
            points.Add(_);
            points.Add(_);
            points.Add(_);

            _topCorner = new Vertex(bbox.Item1, bbox.Item2);
            _bottomCorner = new Vertex(bbox.Item3, bbox.Item4);
            setCorners(_topCorner, _bottomCorner);
        }

        public void setCorners(Vertex topCorner, Vertex bottomCorner)
        {
            var points = recalculatePoints(topCorner, bottomCorner);
            _topCorner = points.Item1;
            _bottomCorner = points.Item3;

            this.points[0] = points.Item1;
            this.points[1] = points.Item2;
            this.points[2] = points.Item3;
            this.points[3] = points.Item4;
        }

        /// <summary>
        /// Generates tuple with points positions.
        /// </summary>
        /// <returns>Tuple of points with old values.</returns>
        private Tuple<
            Vertex,
            Vertex,
            Vertex,
            Vertex
        > recalculatePoints()
        {
            return new Tuple<
                Vertex,
                Vertex,
                Vertex,
                Vertex
            >(
                new Vertex(_topCorner.Item1, _topCorner.Item2),
                new Vertex(_topCorner.Item1, _bottomCorner.Item1),
                new Vertex(_bottomCorner.Item1, _bottomCorner.Item2),
                new Vertex(_topCorner.Item1, _bottomCorner.Item2)
            );
        }

        /// <summary>
        /// Recalculates positions of points in rectangle.
        /// </summary>
        /// <param name="topCorner"></param>
        /// <param name="bottomCorner"></param>
        /// <returns>Tuple of points.</returns>
        private Tuple<
            Vertex,
            Vertex,
            Vertex,
            Vertex
        > recalculatePoints(Vertex topCorner, Vertex bottomCorner) {
            List<int> xs = new List<int>
            {
                topCorner.Item1,
                bottomCorner.Item1
            };

            List<int> ys = new List<int>
            {
                topCorner.Item2,
                bottomCorner.Item2
            };

            xs.Sort();
            ys.Sort();

            topCorner = new Vertex(xs[0], ys[0]);
            bottomCorner = new Vertex(xs[1], ys[1]);

            return new Tuple<
                Vertex,
                Vertex,
                Vertex,
                Vertex
            >(
                new Vertex(topCorner.Item1, topCorner.Item2),
                new Vertex(topCorner.Item1, bottomCorner.Item1),
                new Vertex(bottomCorner.Item1, bottomCorner.Item2),
                new Vertex(bottomCorner.Item1, topCorner.Item2)
            );
        }
    }
}
