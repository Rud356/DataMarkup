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
using Prism.Common;


namespace App.ProjectSettings.ViewModels;

public class ProjectSettingsViewModel : BindableBase, INavigationAware
{
    private IEventAggregator _eventAggregator;
    private IProjectConfigLoader? _config;
    public ICommand ToMarkup { get; }

    public ICommand ApplyChanges { get; }

    public ICommand CancelChanges { get; }

    public ICommand AddClassCommand { get; }
    public ICommand DeleteClassCommand { get; }

    public ObservableString InputedClassName { get; } = new ObservableString { Value = string.Empty };
    public ObservableCollection<ObservableString> editableClassNamesProxy { get; }
    private List<string> originalClassesList = new List<string>();

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
        editableClassNamesProxy = new ObservableCollection<ObservableString>();
        regionManager.RequestNavigate(Regions.MainRegion, Navigation.MarkupPage);
        ToMarkup = new DelegateCommand(() =>
        {
            regionManager.RequestNavigate("MainRegion", "MarkupWindow");
        }
        );
        _eventAggregator.GetEvent<ConfigSharingEvent>().Subscribe(HandleGettingConfig);

        AddClassCommand = new DelegateCommand(
            () =>
            {
                if (InputedClassName.Value is not null && InputedClassName.Value.Length > 0)
                {
                    editableClassNamesProxy.Add(new ObservableString { Value = InputedClassName.Value });
                    _config.ProjectConfigObj.MarkupClasses.Add(InputedClassName.Value);
                }
            }
        );

        DeleteClassCommand = new DelegateCommand<ObservableString>(DeleteClass);
        CancelChanges = new DelegateCommand(() => applyChanges = false);
        ApplyChanges = new DelegateCommand(() => applyChanges = true);
    }

    private void HandleGettingConfig(IProjectConfigLoader config)
    {
        _config = config;
        editableClassNamesProxy.Clear();
        var list = config.ProjectConfigObj.MarkupClasses.ToList();
        originalClassesList = list;

        for ( var i = 0; i < list.Count; i++ )
        {
            ObservableString str = new ObservableString { Value = list[i] };
            str.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Value")
                {
                    ObservableString os = sender as ObservableString;
                    int index = editableClassNamesProxy.IndexOf(os);
                    if (os != null && string.Empty == os.Value)
                    {
                        // Notify that class has been deleted and it needs to be handles
                        _config.ProjectConfigObj.removeMarkupClass("[Удалено]");
                        _eventAggregator.GetEvent<MarkupClassDeletedEvent>().Publish(index);
                        
                        return;
                    }

                    _config.ProjectConfigObj.renameMarkupClassTo(Config.ProjectConfigObj.MarkupClasses[index], os.Value);
                }
            };
            editableClassNamesProxy.Add(str);
        }
    }

    private void DeleteClass(ObservableString className)
    {
        className.Value = "[Удалено]";

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
        if (applyChanges && _config is not null)
        {
            foreach (var item in editableClassNamesProxy)
            {
                if (item.Value == "[Удалено]")
                {
                    // Send deletion event to cleanup classes
                    item.Value = string.Empty;
                }
            }

            _config?.SaveConfig(_config.ProjectConfigObj);
            navigationContext.Parameters.Add("projectConfig", _config?.ProjectConfigObj);
        }

        else if (_config is not null)
        {
            // Restore settings
            _config.ProjectConfigObj.MarkupClasses.Clear();
            originalClassesList.ForEach(_config.ProjectConfigObj.MarkupClasses.Add);
        }
    }
}
