using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    internal interface IMarkupFigure
    {
        string AssignedClassName { get; set; }
        List<Tuple<int, int>> getFiguresPoints();
    }
}
