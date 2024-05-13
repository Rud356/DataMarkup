using Prism.Ioc;
using Prism.Modularity;
using App.MarkupProject.Views;
using App.MarkupProject;
using App.Shared;
using Prism.Regions;

namespace App.MarkupProject
{
    public class ProjectModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MarkupWindow>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
