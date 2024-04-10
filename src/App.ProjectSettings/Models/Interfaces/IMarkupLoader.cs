﻿using App.ProjectSettings.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    public interface IMarkupLoader
    {
        public IList<Tuple<string, IList<MarkupDTO>>> LoadImages(IMarkupFormatter formatter, IList<string> imagesPaths);
        public Tuple<string, IList<MarkupDTO>> LoadImageMarkup(IMarkupFormatter formatter, string imagePath);
    }
}
