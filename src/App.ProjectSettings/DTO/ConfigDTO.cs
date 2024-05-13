using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ProjectSettings.DTO
{
    internal class ConfigDTO
    {
        [JsonProperty("ProjectName")]
        public string ProjectName { get; set; }
        [JsonProperty("dateFormatter")]
        public string dataFormatter { get; set; }
        [JsonProperty("excludedImages")]
        public List<string> excludedImages { get; set; }
        [JsonProperty("MarkupClasses")]
        public List<string> markupClasses { get; set; }

        public ConfigDTO(string projectName, string dataFormatter, List<string> excludedImages, List<string> markupClasses)
        {
            ProjectName = projectName;
            this.dataFormatter = dataFormatter;
            this.excludedImages = excludedImages;
            this.markupClasses = markupClasses;
        }
    }
}
