using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    public interface IProjectConfig
    {
        public string ProjectPath { get; }
        public string ProjectConfigPath { get; }
        public string ProjectName { get; set; }

        public ISet<string> MarkupClasses { get; }
        public IMarkupFormatter DataFormat { get; set; }
        public IList<string> ExcludedImages { get; }

        public virtual void addMarkupClass(string markupClassName) { }
        public virtual void removeMarkupClass(string markupClassName) { }
        public virtual void renameMarkupClassTo(string markupClassName, string newMarkupClassName) { }

        public virtual void excludeImage(string ImagePath) { }
        public virtual void includeImage(string ImagePath) { }
    }
}
