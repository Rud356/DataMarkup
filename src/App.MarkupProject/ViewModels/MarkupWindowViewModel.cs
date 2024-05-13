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
using System.Windows.Controls;
using App.MarkupProject.Models;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using App.Shared.Events;
using Prism.Events;
using App.ProjectSettings.Models;
using App.ProjectSettings.Models.Interfaces;


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
        private PolygonEditMode _editMode = PolygonEditMode.None;
        private Polygon _editingPolygon = null;
        private Point? _firstPoint = null; // Добавляем для хранения первой точки прямоугольника
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

        private IMarkupProject _project;

        private MarkupImage _selectedImage;


        public MarkupImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
                UpdateCanvas();
            }
        }

        public Canvas ImageCanvas { get; }

        public MarkupWindowViewModel(IRegionManager regionManager, Canvas imageCanvas)
        {
            

            GoBackCommand = new DelegateCommand(() =>
            {
                regionManager.RequestNavigate("MainRegion", "MainView");
            });
            _project = ExecuteLoadProject();

            if (_project == null)
                // Go back
                regionManager.RequestNavigate("MainRegion", "MainView");

            PolygonToolCommand = new DelegateCommand(ExecutePolygonTool);
            RectangleToolCommand = new DelegateCommand(ExecuteRectangleTool);
            DeleteToolCommand = new DelegateCommand(ExecuteDeleteTool);
            ChooseFigureToolCommand = new DelegateCommand(ExecuteChooseFigureTool);
            MoveFigureToolCommand = new DelegateCommand(ExecuteMoveFigureTool);
            MovePointsToolCommand = new DelegateCommand(ExecuteMovePointsTool);
            ImageCanvas = imageCanvas;
        }

        private void ExecutePolygonTool()
        {
            SelectedTool = MarkupTool.Polygon;
            // Обработка команды инструмента "Полигон"
        }

        private void ExecuteRectangleTool()
        {
            SelectedTool = MarkupTool.Rectangle;

            // Очищаем предыдущие обработчики событий, чтобы избежать дублирования
            ImageCanvas.MouseLeftButtonDown -= ImageCanvas_MouseLeftButtonDown;

            // Добавляем обработчик события нажатия на холст для рисования прямоугольника
            ImageCanvas.MouseLeftButtonDown += ImageCanvas_MouseLeftButtonDown;
            
            var imagePath = "C:\\Users\\Rud356-pc\\Documents\\Projects source code\\DataMarkup\\src\\App.MarkupProject\\Images\\testImage.png";
            SelectedImage = new MarkupImage(imagePath);
            UpdateCanvas();
        }


        // TODO: переделать, только на событиях
        private void ImageCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Получаем координаты клика мыши относительно холста
            Point position = e.GetPosition((IInputElement)sender);

            if (_firstPoint == null)
            {
                // Сохраняем координаты первой точки прямоугольника
                _firstPoint = position;
            }
            else
            {
                // Создаем прямоугольник с углами в первой и второй точках
                double left = Math.Min(_firstPoint.Value.X, position.X);
                double top = Math.Min(_firstPoint.Value.Y, position.Y);
                double width = Math.Abs(position.X - _firstPoint.Value.X);
                double height = Math.Abs(position.Y - _firstPoint.Value.Y);

                // Создаем прямоугольник
                var rectangle = new System.Windows.Shapes.Rectangle
                {
                    Width = width,
                    Height = height,
                    Stroke = Brushes.Red, // Выбираем цвет обводки прямоугольника
                    StrokeThickness = 2, // Выбираем толщину обводки прямоугольника
                    Margin = new Thickness(left, top, 0, 0) // Устанавливаем позицию прямоугольника
                };

                // Добавляем прямоугольник на холст
                ImageCanvas.Children.Add(rectangle);

                // Сбрасываем первую точку прямоугольника для создания нового прямоугольника
                _firstPoint = null;
            }
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
        private void UpdateCanvas()
        {
            if (SelectedImage != null)
            {
                // TODO: починить размеры в xaml
                ImageCanvas.UpdateLayout();

                // Отображение элементов разметки
                MarkupDisplayUpdate();
            }
        }

        private void MarkupDisplayUpdate()
        {
            // Отображение элементов разметки
            foreach (IMarkupFigure figure in SelectedImage.Markup)
            {
                // ТУТ ЛОГИКА
            }
        }

        private IMarkupProject ExecuteLoadProject(bool showDialog = true)
        {
            // Открыть диалоговое окно выбора папки
            var dialog = new OpenFileDialog();
            dialog.Title = "Выберите папку проекта";
            dialog.Filter = "Папки|.";
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.FileName = "Выберите папку";

            string selectedFolder;
            IProjectConfigLoader configLoader;
            IMarkupProject markupProject;

            bool? result = true;

            if (showDialog)
            {
                result = dialog.ShowDialog();
            }

            if (result == true)
            {
                selectedFolder = Path.GetDirectoryName(dialog.FileName);
                // TODO: null check
                // TODO: добавить вопрос о создании нового проекта, если не обнаружен в папке
                // Получить список файлов в выбранной папке
                try
                {
                    configLoader = new ProjectConfigLoader(selectedFolder);
                    markupProject = new Models.MarkupProject(configLoader);
                }
                // TODO: сделать правильней по логике, сейчас пример создания дефолт конфига
                catch (Exception e)
                {
                    File.WriteAllText(selectedFolder + "\\config.cfg", ProjectConfigLoader.DefaultConfigString());
                    return ExecuteLoadProject();
                }

                // Обработать каждый файл, как необходимо для вашего приложения
                return markupProject;
            }
            
            else
            {
                return null;
            }
        }
    }
}
