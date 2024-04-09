using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models.Interfaces
{
    public interface IProjectConfigLoader
    {
        public IProjectConfig ProjectConfigObj { get; }
        public void SaveConfig(IProjectConfig config);
    }
}
