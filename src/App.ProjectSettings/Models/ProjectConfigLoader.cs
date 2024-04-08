using App.MarkupProject.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: finish implementing to be able to load project from folder, and if not found - create new project file
namespace App.MarkupProject.Models
{
    public class ProjectConfigLoader : IProjectConfigLoader
    {
        public IProjectConfig ProjectConfig => throw new NotImplementedException();

        public void saveConfig()
        {
            throw new NotImplementedException();
        }

        private string defaultConfigString()
        {
            return "";
        }
    }
}
