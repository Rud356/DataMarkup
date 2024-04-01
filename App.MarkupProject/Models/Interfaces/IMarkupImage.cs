using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    internal interface IMarkupImage
    {
        string ImagePath { get; }
        bool IsIncludedInExport { get; set; }

        void AddFigureOnImage();
        void RemoveFigureFromImage(int figureIndex);

        List<IMarkupImage> GetImagesMarkup();
    }
}
