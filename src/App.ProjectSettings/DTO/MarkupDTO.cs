using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ProjectSettings.DTO
{
    public enum MarkupFigureType
    {
        bbox = 0,
        polygon = 1
    }

    public class MarkupDTO
    {
        public int AssignedClassID { get; }
        public MarkupFigureType MarkupType { get; }
        public IList<Tuple<int, int>> Points { get; }

        public MarkupDTO(int assignedClassID, MarkupFigureType markupType, IList<Tuple<int, int>> points)
        {
            AssignedClassID = assignedClassID;
            MarkupType = markupType;
            Points = points;
        }
    }
}
