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
using App.ProjectSettings.Models.Interfaces;

// TODO: finish implementing to be able to load project from folder, and if not found - create new project file
namespace App.ProjectSettings.Models
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

            ConfigDTO? cfg = JsonConvert.DeserializeObject<ConfigDTO>(File.ReadAllText(projectConfigPath));
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

            if (cfg.dataFormatter is null)
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
                SaveConfigDTO(cfg, projectConfigPath);
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
            ConfigDTO cfg = new ConfigDTO(
                config.ProjectName,
                SupportedFormats.SupportedFormats.FormatToEnum(config.DataFormat).ToString(),
                config.ExcludedImages.ToList(),
                config.MarkupClasses.ToList()
            );
            SaveConfigDTO(cfg, config.ProjectConfigPath);
        }

        private void SaveConfigDTO(ConfigDTO config, string saveTo)
        {
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(saveTo, json);
        }

        public static string DefaultConfigString()
        {
            return /*lang=json*/ "{\"ProjectName\": \"Default Name\", \"excludedImages\": [], \"markupClasses\": [\"Default\"], \"dataFormatter\": \"CocoDataset\"}";
        }
    }
}
