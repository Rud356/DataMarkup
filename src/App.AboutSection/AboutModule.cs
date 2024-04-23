using Prism.Ioc;
using Prism.Modularity;
using App.AboutSection.Views;
using App.AboutSection;
using App.Shared;
using Prism.Regions;

namespace SamplePrism2024.About;

public class AboutModule : IModule
{
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<AboutView>();
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
        
    }
}
