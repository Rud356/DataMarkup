using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using App;
using Prism.Regions;

namespace App.MarkupProject.ViewModels
{
    internal class MarkupWindowViewModel : BindableBase
    {
        public ICommand GoBackCommand { get; set; }

        public MarkupWindowViewModel(IRegionManager regionManager)
        {
            GoBackCommand = new DelegateCommand(() =>
            {
                regionManager.RequestNavigate("MainRegion", "MainView");
            });
        }
    }
}
