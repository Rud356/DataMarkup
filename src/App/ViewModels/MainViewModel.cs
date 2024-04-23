using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Prism.Regions;
using App.AboutSection.Views;
namespace App.ViewModels
{
    internal class MainViewModel
    {
        public ICommand TestCommand { get; set; }
        public MainViewModel(IRegionManager navigation)
        {
            TestCommand = new DelegateCommand(() => {
                navigation.RequestNavigate("MainRegion", "AboutView");
            });
        }
    }
}
