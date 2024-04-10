using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.MarkupProject.Models.Interfaces
{
    public interface IMarkupProject
    {
        public IProjectConfigLoader ConfigLoader { get; }
        public IMarkupFormatter Formatter { get; set; }
        public ObservableCollection<IMarkupImage> Images { get; }
        public string Name { get; set; }

        public void ExportProject();
    }
}
