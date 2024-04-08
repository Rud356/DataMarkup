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
        public string AssignedClassName { get; set; }
        public bool IsHidden { get; set; }

        public ObservableCollection<Tuple<int, int>> Points { get; }
    }
}
