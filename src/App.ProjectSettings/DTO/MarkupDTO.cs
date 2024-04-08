using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ProjectSettings.DTO
{
    public class MarkupDTO
    {
        public string AssignedClassName { get; }
        public string MarkupType { get; }
        public IList<Tuple<int, int>> Points { get; }

        public MarkupDTO(string assignedClassName, string markupType, IList<Tuple<int, int>> points)
        {
            AssignedClassName = assignedClassName;
            MarkupType = markupType;
            Points = points;
        }
    }
}
