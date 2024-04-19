using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using App.AboutSection;
using App.MarkupProject;
using App.ViewModels;
using App.Views;

namespace App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class MainApp
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<AppViewModel>();
        /*containerRegistry
            .RegisterSingleton<AboutModule>();*/
    }
    
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        /*moduleCatalog
            .AddModule<AboutModule>();*/
    }

    protected override Window CreateShell() => Container.Resolve<ShellView>();
}