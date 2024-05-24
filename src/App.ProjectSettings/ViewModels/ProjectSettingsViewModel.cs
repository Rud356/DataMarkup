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

public class ProjectSettingsViewModel : BindableBase
{
    public ICommand ToMarkup { get; }

    public ProjectSettingsViewModel(IRegionManager regionManager)
    {
        regionManager.RequestNavigate(Regions.MainRegion, Navigation.MarkupPage);
        ToMarkup = new DelegateCommand(() =>
        {
            regionManager.RequestNavigate("MainRegion", "MarkupWindow");
        }
        );
    }
}
