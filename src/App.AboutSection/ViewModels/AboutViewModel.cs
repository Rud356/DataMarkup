using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace App.AboutSection.ViewModels
{
    internal class AboutViewModel : BindableBase
    {
        //public ICommand TestCommand { get; set; }
        //public AboutViewModel()
        //{
        //    TestCommand = new DelegateCommand(() => { 
        //        MessageBox.Show("123"); 
        //    }) ;
        //}
        public ICommand GoBackCommand { get; set; }

        public AboutViewModel(IRegionManager regionManager)
        {
            GoBackCommand = new DelegateCommand(() =>
            {
                regionManager.RequestNavigate("MainRegion", "MainView");
            });
        }
    }
}
