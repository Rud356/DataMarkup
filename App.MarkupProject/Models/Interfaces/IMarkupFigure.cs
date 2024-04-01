using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    public interface IMarkupFigure
    {
        public string AssignedClassName { get; set; }
        public bool IsHidden { get; set; }

        public List<Tuple<int, int>> GetFiguresPoints();
    }
}
