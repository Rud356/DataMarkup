using App.MarkupProject.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models
{
    public class ProjectConfig : IProjectConfig
    {
        private string _projectPath;
        private IList<string> _excludedImages;
        private ISet<string> _markupClasses;

        public ProjectConfig(
                string projectPath,
                string projectName,
                IMarkupFormatter dataFormatter,
                IList<string> excludedImages,
                ISet<string> markupClasses
            )
        {
            _projectPath = projectPath;
            _excludedImages = excludedImages;
            _markupClasses = markupClasses;
            ProjectName = projectName;
            DataFormat = dataFormatter;
        }
        
        public string ProjectPath => _projectPath;

        public string ProjectConfigPath => throw new NotImplementedException();

        public string ProjectName { get; set; }
        public IMarkupFormatter DataFormat { get; set; }

        public ISet<string> MarkupClasses { get => _markupClasses; }

        public IList<string> ExcludedImages { get => _excludedImages; }

        public void addMarkupClass(string markupClassName)
        {
            _markupClasses.Add(markupClassName);
        }

        public void removeMarkupClass(string markupClassName)
        {
            _markupClasses.Remove(markupClassName);
        }

        public void renameMarkupClassTo(string markupClassName, string newMarkupClassName)
        {
            _markupClasses.Remove(markupClassName);
            _markupClasses.Add(newMarkupClassName);
        }

        public void excludeImage(string ImagePath)
        {
            if (ImagePath != null && ExcludedImages.IndexOf(ImagePath) != -1)
            {
                _excludedImages.Add(ImagePath);
            }
        }

        public void includeImage(string ImagePath)
        {
            _excludedImages.Remove(ImagePath);
        }
    }
}
