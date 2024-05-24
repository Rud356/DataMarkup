using Prism.Ioc;
using Prism.Modularity;
using App.ProjectSettings.Views;
using App.ProjectSettings;
using App.Shared;
using Prism.Regions;

namespace SamplePrism2024.About;

public class SettingsModule : IModule
{
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<ProjectSettingsWindow>();
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
        
    }
}
