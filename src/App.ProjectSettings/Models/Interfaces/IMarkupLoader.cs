using App.ProjectSettings.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ProjectSettings.Models.Interfaces
{
    public interface IMarkupLoader
    {
        public IList<Tuple<string, Tuple<int, int>, IList<MarkupDTO>>> LoadMarkup(
            string markupText
        );
    }
}
