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
using App.Shared.Events;
using Microsoft.Win32;
using Prism.Events;
using System.IO;
namespace App.ViewModels
{
    internal class MainViewModel
    {
        public ICommand TestCommand { get; set; }
        public ICommand LoadProjectCommand { get; set; }
        public ICommand OpenMarkupWindowCommand { get; set; }
        public IEventAggregator EventAggregator { get; }
        public MainViewModel(IEventAggregator eventaggregator, IRegionManager navigation)
        {
            EventAggregator = eventaggregator;
            EventAggregator.GetEvent<ConfigurationEvent>().Subscribe(
                v =>
                {
                    navigation.RequestNavigate("MainRegion", "MarkupWindow");
                }
                );
            LoadProjectCommand = new DelegateCommand(ExecuteLoadProject);
            TestCommand = new DelegateCommand(() => {
                navigation.RequestNavigate("MainRegion", "AboutView");
            });
            
            OpenMarkupWindowCommand = new DelegateCommand(() => // мб убрать
            {
                navigation.RequestNavigate("MainRegion", "MarkupWindow");
            });
        }
        private void ExecuteLoadProject()
        {
            // Открыть диалоговое окно выбора папки
            var dialog = new OpenFileDialog();
            dialog.Title = "Выберите папку проекта";
            dialog.Filter = "Папки|.";
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.FileName = "Выберите папку";

            if (dialog.ShowDialog() == true)
            {
                string selectedFolder = Path.GetDirectoryName(dialog.FileName);
                EventAggregator.GetEvent<ConfigurationEvent>().Publish(selectedFolder);
                // Получить список файлов в выбранной папке


                try { }
                catch { }

                // Обработать каждый файл, как необходимо для вашего приложения

            }
        }
    }
}
