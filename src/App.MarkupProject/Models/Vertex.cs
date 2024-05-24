using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models
{
    public class Vertex : ReactiveObject, IComparable<Vertex>
    {
        [Reactive]
        public int X {  get; set; }
        [Reactive]
        public int Y { get; set; }
        [Reactive]
        public int Item1 { get =>  X; set => X = value; }
        [Reactive]
        public int Item2 { get => Y; set => Y = value; }

        [Reactive]
        public bool IsSelected { get; set; } = false;

        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int CompareTo(Vertex? other)
        {
            if (other == null) return 1;

            if (this.X < other.X)
                return -1;

            if (this.Y < other.Y)
                return 1;

            return 0;
        }

        public Tuple<int, int> ToTuple()
        {
            return new Tuple<int, int>(X, Y);
        }
    }
}
