using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    internal interface IProjectConfig
    {
        public string ProjectName { get; set; }
        public IMarkupFormatter DataFormat { get; set; }
        public List<string> ExcludedImages { get; }

        public void addMarkupClass(string markupClassName);
        public void removeMarkupClass(string markupClassName);
        public void renameMarkupClassTo(string markupClassName, string newMarkupClassName);

        public void excludeImage(string ImagePath);
        public void includeImage(string ImagePath);
    }
}
