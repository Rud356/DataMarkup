using Microsoft.VisualBasic;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace App.MarkupProject.Models
{
    /// <summary>
    /// Points go from top left corner to bottom left corner in clockwise order.
    /// Points are given in (x, y) format, with ints repserenting pixels poisiton on image.
    /// </summary>
    public class Rectangle : Polygon
    {
        private ObservableCollection<Tuple<int, int>> _cachedPositions; 
        private Tuple<int, int> _topCorner, _bottomCorner;

        public Rectangle(ref ObservableCollection<string> labels, Tuple<int, int, int, int> bbox, int classID) : base(ref labels, classID)
        {
            var _ = new Tuple<int, int>(0, 0);
            base.Points.Add(_);
            base.Points.Add(_);
            base.Points.Add(_);
            base.Points.Add(_);
            base.AssignedClassID = classID;
            _topCorner = new Tuple<int, int>(bbox.Item1, bbox.Item2);
            _bottomCorner = new Tuple<int, int>(bbox.Item3, bbox.Item4);
            setCorners(_topCorner, _bottomCorner);
        }

        public void setCorners(Tuple<int, int> topCorner, Tuple<int, int> bottomCorner)
        {
            var points = recalculatePoints(topCorner, bottomCorner);
            _topCorner = points.Item1;
            _bottomCorner = points.Item3;

            base.Points[0] = points.Item1;
            base.Points[1] = points.Item2;
            base.Points[2] = points.Item3;
            base.Points[3] = points.Item4;
        }

        /// <summary>
        /// Generates tuple with points positions.
        /// </summary>
        /// <returns>Tuple of points with old values.</returns>
        private Tuple<
            Tuple<int, int>,
            Tuple<int, int>,
            Tuple<int, int>,
            Tuple<int, int>
        > recalculatePoints()
        {
            return new Tuple<
                Tuple<int, int>,
                Tuple<int, int>,
                Tuple<int, int>,
                Tuple<int, int>
            >(
                new Tuple<int, int>(_topCorner.Item1, _topCorner.Item2),
                new Tuple<int, int>(_topCorner.Item1, _bottomCorner.Item1),
                new Tuple<int, int>(_bottomCorner.Item1, _bottomCorner.Item2),
                new Tuple<int, int>(_bottomCorner.Item1, _topCorner.Item2)
            );
        }

        /// <summary>
        /// Recalculates positions of points in rectangle.
        /// </summary>
        /// <param name="topCorner"></param>
        /// <param name="bottomCorner"></param>
        /// <returns>Tuple of points.</returns>
        private Tuple<
            Tuple<int, int>,
            Tuple<int, int>,
            Tuple<int, int>,
            Tuple<int, int>
        > recalculatePoints(Tuple<int, int> topCorner, Tuple<int, int> bottomCorner) {
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

            topCorner = new Tuple<int, int>(xs[0], ys[0]);
            bottomCorner = new Tuple<int, int>(xs[1], ys[1]);

            return new Tuple<
                Tuple<int, int>,
                Tuple<int, int>,
                Tuple<int, int>,
                Tuple<int, int>
            >(
                new Tuple<int, int>(topCorner.Item1, topCorner.Item2),
                new Tuple<int, int>(topCorner.Item1, bottomCorner.Item1),
                new Tuple<int, int>(bottomCorner.Item1, bottomCorner.Item2),
                new Tuple<int, int>(bottomCorner.Item1, topCorner.Item2)
            );
        }
    }
}
