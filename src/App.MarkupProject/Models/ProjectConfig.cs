using App.MarkupProject.Models.Interfaces;
using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace App.MarkupProject.Models
{
    public class ProjectConfig : ReactiveObject, IProjectConfig
    {
        private string _projectPath;
        private ObservableCollection<string> _excludedImages;
        private ObservableCollection<string> _markupClasses;

        public ProjectConfig(
                string projectPath,
                string projectName,
                IMarkupFormatter dataFormatter,
                ObservableCollection<string> excludedImages,
                ObservableCollection<string> markupClasses
            )
        {
            _projectPath = projectPath;
            _excludedImages = excludedImages;
            _markupClasses = markupClasses;
            ProjectName = projectName;
            DataFormat = dataFormatter;
        }

        [Reactive] public string ProjectPath => _projectPath;

        [Reactive] public string ProjectConfigPath => throw new NotImplementedException();

        [Reactive] public string ProjectName { get; set; }
        [Reactive] public IMarkupFormatter DataFormat { get; set; }

        [Reactive] public ObservableCollection<string> MarkupClasses { get => _markupClasses; }

        [Reactive] public ObservableCollection<string> ExcludedImages { get => _excludedImages; }

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
