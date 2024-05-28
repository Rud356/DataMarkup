using Prism.Commands;
using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.IO;
using App.ProjectSettings.Models.Interfaces;
using System.Windows.Input;
using DynamicData;

namespace App.ProjectSettings.Models
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

        [Reactive]
        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                if (Directory.Exists(value)) _projectPath = value;
                else
                {
                    throw new DirectoryNotFoundException("Path can not be project directory!");
                }
            }
        }

        [Reactive] public string ProjectConfigPath => Path.Combine(_projectPath, "config.cfg");

        [Reactive] public string ProjectName { get; set; }
        [Reactive] public IMarkupFormatter DataFormat { get; set; }

        [Reactive] public ref ObservableCollection<string> MarkupClasses { get => ref _markupClasses; }

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
            _markupClasses.Replace(markupClassName, newMarkupClassName);
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
