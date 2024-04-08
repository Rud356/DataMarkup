﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    public interface IMarkupProject
    {
        public IProjectConfigLoader ConfigLoader { get; }
        public IMarkupFormatter Formatter { get; set; }
        public IList<IMarkupImage> Images { get; }
        public string Name { get; }

        public void ExportProject();
    }
}
