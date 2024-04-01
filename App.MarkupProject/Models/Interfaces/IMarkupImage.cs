using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    public interface IMarkupImage
    {
        public string ImagePath { get; }
        public bool IsIncludedInExport { get; set; }

        public void AddFigureOnImage(IMarkupFigure figure);
        public void RemoveFigureFromImage(int figureIndex);

        public List<IMarkupFigure> GetImagesMarkup();
    }
}
