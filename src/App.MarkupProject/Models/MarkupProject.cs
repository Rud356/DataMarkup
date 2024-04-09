using App.MarkupProject.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.MarkupProject.Models
{
    internal class MarkupProject : IMarkupProject
    {
        public IProjectConfigLoader ConfigLoader => throw new NotImplementedException();

        public IMarkupFormatter Formatter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IList<IMarkupImage> Images => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public void ExportProject()
        {
            throw new NotImplementedException();
        }
    }
}
