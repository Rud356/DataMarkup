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
using Prism.Events;
using App.ProjectSettings.Events;
using ReactiveUI.Fody.Helpers;


namespace App.ProjectSettings.ViewModels;

public class ProjectSettingsViewModel : BindableBase, INavigationAware
{
    private IEventAggregator _eventAggregator;
    private IProjectConfigLoader? _config;
    public ICommand ToMarkup { get; }

    public ICommand DeleteClassCommand { get; }

    [Reactive]
    public IProjectConfigLoader? Config
    {
        get { return _config; }
        set
        {
            this.SetProperty(ref _config, value);
        }
    }

    private bool applyChanges = false;

    public ProjectSettingsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        regionManager.RequestNavigate(Regions.MainRegion, Navigation.MarkupPage);
        ToMarkup = new DelegateCommand(() =>
        {
            regionManager.RequestNavigate("MainRegion", "MarkupWindow");
        }
        );
        _eventAggregator.GetEvent<ConfigSharingEvent>().Subscribe(HandleGettingConfig);
        DeleteClassCommand = new DelegateCommand<string>(DeleteClass);
    }

    private void HandleGettingConfig(IProjectConfigLoader config)
    {
        _config = config;
    }

    private void DeleteClass(string className)
    {
        Config?.ProjectConfigObj.removeMarkupClass(className);
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        // Now you can use _configLoader to edit the config
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        // Save the changes to the config before navigating away
        if (!applyChanges && _config is not null)
        {
            _config?.SaveConfig(_config.ProjectConfigObj);
            navigationContext.Parameters.Add("projectConfig", _config?.ProjectConfigObj);
        }
    }
}
