using App.ProjectSettings.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: Make interface for markup format loading and storing on disk

namespace App.MarkupProject.Models.Interfaces
{
    public interface IMarkupFormatter
    {
        public void saveMarkup(string saveTo, IList<ImageDTO> imagesMarkup);
    }
}
