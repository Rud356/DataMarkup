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
    public class Rectangle : Polygon, IMarkupFigure
    {
        private Vertex _topCorner, _bottomCorner;

        public Rectangle(ref ObservableCollection<string> labels, Tuple<int, int, int, int> bbox, int classID) : base(ref labels, classID, true)
        {
            AssignedClassID = classID;
            _topCorner = new Vertex(bbox.Item1, bbox.Item2);
            _bottomCorner = new Vertex(bbox.Item3, bbox.Item4);
            setCorners(_topCorner, _bottomCorner);
        }

        public void setCorners(Vertex topCorner, Vertex bottomCorner)
        {
            var points = recalculatePoints(topCorner, bottomCorner);
            _topCorner = points.Item1;
            _bottomCorner = points.Item3;

            base.Points.Add(points.Item1);
            base.Points.Add(points.Item2);
            base.Points.Add(points.Item3);
            base.Points.Add(points.Item4);
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
