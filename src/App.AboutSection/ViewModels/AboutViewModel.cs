using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace App.AboutSection.ViewModels
{
    internal class AboutViewModel : BindableBase
    {
        public ICommand TestCommand { get; set; }
        public AboutViewModel()
        {
            TestCommand = new DelegateCommand(() => { 
                MessageBox.Show("123"); 
            }) ;
        }
    }
}
