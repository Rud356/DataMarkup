using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Prism.Regions;
using App.ProjectSettings.Views;
using App.MarkupProject.Views;
using App.AboutSection.Views;
using App.Shared.Events;
using Microsoft.Win32;
using Prism.Events;
using System.IO;
namespace App.ViewModels
{
    internal class MainViewModel
    {
        public ICommand TestCommand { get; set; }
        public ICommand OpenMarkupWindowCommand { get; set; }
        public IEventAggregator EventAggregator { get; }
        public MainViewModel(IEventAggregator eventaggregator, IRegionManager navigation)
        {
            EventAggregator = eventaggregator;
            TestCommand = new DelegateCommand(() => {
                navigation.RequestNavigate("MainRegion", "AboutView");
            });
            
            OpenMarkupWindowCommand = new DelegateCommand(() =>
            {
                navigation.RequestNavigate("MainRegion", "MarkupWindow");
            });
        }
    }
}
