using Prism.Ioc;
using Prism.Modularity;
using App.AboutSection.Views;
using App.AboutSection;

namespace SamplePrism2024.About;

public class AboutModule : IModule
{
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.Register<AboutModule>();
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
        //containerProvider
        //    .Resolve<IRegionManager>()
        //    .RegisterViewWithRegion(Regions.MainRegion, nameof(AboutView));
    }
}
