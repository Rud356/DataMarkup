﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    public interface IMarkupImage
    {
        public string ImagePath { get; }

        public string Name { get; }

        public bool IsIncludedInExport { get; set; }

        public Tuple<int, int> Resolution { get; }

        public ObservableCollection<IMarkupFigure> Markup { get; }
    }
}
