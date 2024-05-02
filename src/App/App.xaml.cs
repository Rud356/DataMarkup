﻿using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using App.AboutSection;
using App.MarkupProject;
using App.ViewModels;
using App.Views;
using SamplePrism2024.About;
using App.AboutSection.Views;
using App.MarkupProject.Views;

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
        containerRegistry.RegisterForNavigation<MainView>();
    }
    
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog
            .AddModule<AboutModule>()
            .AddModule<MarkupModule>();
    }
    
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Container
            .Resolve<IRegionManager>()
            .RegisterViewWithRegion("MainRegion", nameof(MainView));
    }
    protected override Window CreateShell() => Container.Resolve<ShellView>();
}