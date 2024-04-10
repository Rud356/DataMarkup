using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ProjectSettings.DTO
{
    public class ImageDTO
    {
        public string ImagePath { get; }
        public IList<MarkupDTO> MarkupDTOs { get; }
        public Tuple<int, int> Resolution { get; }

        public ImageDTO(string imagePath, Tuple<int, int> resolution, IList<MarkupDTO> markupDTOs)
        {
            ImagePath = imagePath;
            MarkupDTOs = markupDTOs;
            Resolution = resolution;
        }
    }
}
