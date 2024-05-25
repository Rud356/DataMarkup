using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using Prism.Regions;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.ComponentModel;
using App.ProjectSettings.Models;
using App.ProjectSettings.Models.Interfaces;
using System.Collections.ObjectModel;
using App.ProjectSettings.DTO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime;
using App.Shared;


namespace App.ProjectSettings.ViewModels;

public class ProjectSettingsViewModel : BindableBase, INavigationAware
{
    private IProjectConfigLoader _configLoader;
    public ICommand ToMarkup { get; }

    public IProjectConfigLoader ConfigLoader => _configLoader;

    public ProjectSettingsViewModel(IRegionManager regionManager)
    {
        regionManager.RequestNavigate(Regions.MainRegion, Navigation.MarkupPage);
        ToMarkup = new DelegateCommand(() =>
        {
            var parameters = new NavigationParameters();
            parameters.Add("projectConfig", _configLoader?.ProjectConfigObj);
            regionManager.RequestNavigate("MainRegion", "MarkupWindow", parameters);
        }
        );
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _configLoader = navigationContext.Parameters.GetValue<IProjectConfigLoader>("configLoader");
        // Now you can use _configLoader to edit the config
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        // Save the changes to the config before navigating away
        // _configLoader.SaveConfig(_configLoader.ProjectConfigObj);
    }
}
