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
using Microsoft.Win32;
using App.MarkupProject.Models.Interfaces;
using App.MarkupProject.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;


namespace App.MarkupProject.ViewModels
{
    public enum MarkupTool
    {
        Polygon,
        Rectangle,
        Delete,
        ChooseFigure,
        MoveFigure,
        MovePoints
    }
    public enum PolygonEditMode
    {
        None,
        New,
        Edit
    }
    internal class MarkupWindowViewModel : BindableBase
    {
        //public ICommand LoadProjectCommand { get; }

        //public MarkupWindowViewModel()
        //{
        //    LoadProjectCommand = new DelegateCommand(ExecuteLoadProject);
        //}

        //private void ExecuteLoadProject()
        //{
        //    // Открыть диалоговое окно выбора папки
        //    var dialog = new OpenFileDialog();
        //    dialog.Title = "Выберите папку проекта";
        //    dialog.Filter = "Папки|.";
        //    dialog.CheckFileExists = false;
        //    dialog.CheckPathExists = true;
        //    dialog.FileName = "Выберите папку";

        //    if (dialog.ShowDialog() == true)
        //    {
        //        string selectedFolder = Path.GetDirectoryName(dialog.FileName);

        //        // Получить список файлов в выбранной папке
        //        string[] files = Directory.GetFiles(selectedFolder);

        //        // Обработать каждый файл, как необходимо для вашего приложения
        //        foreach (string file in files)
        //        {
        //            // Добавьте здесь логику обработки каждого файла
        //        }
        //    }
        //}
        private PolygonEditMode _editMode = PolygonEditMode.None;
        private Polygon _editingPolygon = null;
        private Point _lastMousePosition = new Point(double.NegativeInfinity, double.NegativeInfinity);

        private MarkupTool _selectedTool;
        public MarkupTool SelectedTool
        {
            get { return _selectedTool; }
            set { SetProperty(ref _selectedTool, value); }
        }
        public ICommand PolygonToolCommand { get; }
        public ICommand RectangleToolCommand { get; }
        public ICommand DeleteToolCommand { get; }
        public ICommand ChooseFigureToolCommand { get; }
        public ICommand MoveFigureToolCommand { get; }
        public ICommand MovePointsToolCommand { get; }
        public ICommand GoBackCommand { get; set; }

        public MarkupWindowViewModel(IRegionManager regionManager)
        {

            GoBackCommand = new DelegateCommand(() =>
            {
                regionManager.RequestNavigate("MainRegion", "MainView");
            });

            PolygonToolCommand = new DelegateCommand(ExecutePolygonTool);
            RectangleToolCommand = new DelegateCommand(ExecuteRectangleTool);
            DeleteToolCommand = new DelegateCommand(ExecuteDeleteTool);
            ChooseFigureToolCommand = new DelegateCommand(ExecuteChooseFigureTool);
            MoveFigureToolCommand = new DelegateCommand(ExecuteMoveFigureTool);
            MovePointsToolCommand = new DelegateCommand(ExecuteMovePointsTool);
        }
        private void ExecutePolygonTool()
        {
            SelectedTool = MarkupTool.Polygon;
            // Обработка команды инструмента "Полигон"
        }

        private void ExecuteRectangleTool()
        {
            SelectedTool = MarkupTool.Rectangle;
            // Обработка команды инструмента "Прямоугольник"
        }
        private void ExecuteDeleteTool()
        {
            SelectedTool = MarkupTool.Delete;
        }
        private void ExecuteChooseFigureTool()
        {
            SelectedTool = MarkupTool.ChooseFigure;
        }
        private void ExecuteMoveFigureTool()
        {
            SelectedTool = MarkupTool.MoveFigure;
        }
        private void ExecuteMovePointsTool()
        {
            SelectedTool = MarkupTool.MovePoints;
        }
        
    }
}
