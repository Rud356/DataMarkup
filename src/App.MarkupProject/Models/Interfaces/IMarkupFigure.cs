using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    public interface IMarkupFigure
    {

        public string AssignedClass { get; set; }
        public int AssignedClassID { get; set; }
        public bool IsVisible { get; set; }

        public ref ObservableCollection<string> Labels { get; }

        public ObservableCollection<Vertex> Points { get; }

    }
}
