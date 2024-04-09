using App.MarkupProject.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using App.ProjectSettings.Models.SupportedFormats;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using App.ProjectSettings.DTO;
using App.ProjectSettings.Exceptions;
using App.MarkupProject.Models.SupportedFormats;

// TODO: finish implementing to be able to load project from folder, and if not found - create new project file
namespace App.MarkupProject.Models
{
    public class ProjectConfigLoader : IProjectConfigLoader
    {
        public IProjectConfig ProjectConfigObj { get; }

        public ProjectConfigLoader(string projectDir)
        {
            if (!Directory.Exists(projectDir))
            {
                throw new DirectoryNotFoundException("Not a project dir");
            }

            string projectConfigPath = Path.Combine(projectDir, "config.cfg");
            
            if (!File.Exists(projectConfigPath))
            {
                File.WriteAllText(projectConfigPath, DefaultConfigString());
            }

            ConfigDTO? cfg = JsonConvert.DeserializeObject<ConfigDTO>(projectConfigPath);
            bool isPartiallyMalformed = false;

            if (cfg is null)
            {
                // overwrite with default config
                File.WriteAllText(projectConfigPath, DefaultConfigString());
                throw new MalformedConfigException();
            }

            if (cfg.ProjectName is null)
            {
                cfg.ProjectName = "Project1";
                isPartiallyMalformed = true;
            }

            if (cfg.excludedImages is null)
            {
                cfg.excludedImages = new List<string>();
                isPartiallyMalformed = true;
            }

            if (cfg.markupClasses is null)
            {
                cfg.markupClasses = new List<string>();
                isPartiallyMalformed = true;
            }

            IMarkupFormatter? markupFormat = null;

            if (cfg.markupClasses is null)
            {
                cfg.dataFormatter = "CocoDataset";
                isPartiallyMalformed = true;
            }

            // Get exact data formatter
            var dataFormat = Enum.Parse<EnumSupportedFormats>(cfg.dataFormatter);

            if (dataFormat == EnumSupportedFormats.CocoDataset)
            {
                markupFormat = new CocoDatasetFormat();
            }

            if (markupFormat is null)
                throw new MissedIntializationOfMarkupFormatter();

            if (isPartiallyMalformed)
            {
                SaveConfigDTO(cfg);
            }

            ProjectConfigObj = new ProjectConfig(
                projectDir,
                cfg.ProjectName,
                markupFormat,
                new ObservableCollection<string>(cfg.excludedImages),
                new ObservableCollection<string>(cfg.markupClasses)
            );

            
        }

        public void SaveConfig(IProjectConfig config)
        {
            throw new NotImplementedException();
        }

        private void SaveConfigDTO(ConfigDTO config)
        {
            throw new NotImplementedException();
        }

        private string DefaultConfigString()
        {
            return "";
        }
    }
}
