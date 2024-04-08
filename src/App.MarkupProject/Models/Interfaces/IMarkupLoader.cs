using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    public interface IMarkupLoader
    {
        public IList<IMarkupLoader> LoadImages(IMarkupFormatter formatter, IList<string> imagesPaths);
        public IMarkupImage LoadImage(IMarkupFormatter formatter, string imagePath);
    }
}
