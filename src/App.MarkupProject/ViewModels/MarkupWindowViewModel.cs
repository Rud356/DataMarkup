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

    internal class MarkupWindowViewModel : BindableBase
    {
        private Point? _firstPoint = null; // Первая точка для создания прямоугольника
        private ObservableCollection<Rectangle> _rectangles = new ObservableCollection<Rectangle>();

        public ObservableCollection<Rectangle> Rectangles
        {
            get => _rectangles;
            set => SetProperty(ref _rectangles, value);
        }

        private MarkupTool _selectedTool;
        public MarkupTool SelectedTool
        {
            get { return _selectedTool; }
            set { SetProperty(ref _selectedTool, value); }
        }

        private Canvas _imageCanvas;

        public Canvas ImageCanvas
        {
            get { return _imageCanvas; }
            set { SetProperty(ref _imageCanvas, value); }
        }

        public ICommand PolygonToolCommand { get; }
        public ICommand RectangleToolCommand { get; }
        public ICommand DeleteToolCommand { get; }
        public ICommand ChooseFigureToolCommand { get; }
        public ICommand MoveFigureToolCommand { get; }
        public ICommand MovePointsToolCommand { get; }
        public ICommand GoBackCommand { get; set; }

        public ICommand MouseLeftButtonDownCommand { get; set; }

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

        public MarkupWindowViewModel(IRegionManager regionManager, Canvas imageCanvas)
        {
            MouseLeftButtonDownCommand = new DelegateCommand<MouseButtonEventArgs>(MouseLeftButtonDown);
            GoBackCommand = new DelegateCommand(() =>
            {
                regionManager.RequestNavigate("MainRegion", "MainView");
            });

            _project = ExecuteLoadProject();

            if (_project == null)
                regionManager.RequestNavigate("MainRegion", "MainView");

            PolygonToolCommand = new DelegateCommand(ExecutePolygonTool);
            RectangleToolCommand = new DelegateCommand(ExecuteRectangleTool);
            DeleteToolCommand = new DelegateCommand(ExecuteDeleteTool);
            ChooseFigureToolCommand = new DelegateCommand(ExecuteChooseFigureTool);
            MoveFigureToolCommand = new DelegateCommand(ExecuteMoveFigureTool);
            MovePointsToolCommand = new DelegateCommand(ExecuteMovePointsTool);

            // Присваиваем переданный Canvas ImageCanvas
            ImageCanvas = imageCanvas;

            Rectangles.CollectionChanged += (s, e) => UpdateCanvas();
        }

        private void ExecutePolygonTool()
        {
            SelectedTool = MarkupTool.Polygon;
        }

        private void ExecuteRectangleTool()
        {
            SelectedTool = MarkupTool.Rectangle;

            // Подписываемся на событие MouseLeftButtonDown

            var imagePath = "C:\\Users\\MSI RTX\\Desktop\\Учеба\\NewDataMarkup\\DataMarkup\\src\\App.MarkupProject\\Images\\testImage.png";
            SelectedImage = new MarkupImage(imagePath);
            UpdateCanvas();
        }

        private void MouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (SelectedTool == MarkupTool.Rectangle)
            {
                Point position = e.GetPosition(ImageCanvas);

                if (_firstPoint == null)
                {
                    _firstPoint = position;
                }
                else
                {
                    var topLeft = new Tuple<int, int>((int)_firstPoint.Value.X, (int)_firstPoint.Value.Y);
                    var bottomRight = new Tuple<int, int>((int)position.X, (int)position.Y);
                    var rectangle = new Rectangle(new Tuple<int, int, int, int>(topLeft.Item1, topLeft.Item2, bottomRight.Item1, bottomRight.Item2), 0);

                    Rectangles.Add(rectangle);
                    _firstPoint = null; // Сбросить первую точку для следующего прямоугольника
                }
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
                ImageCanvas.Children.Clear();

                var image = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri(SelectedImage.ImagePath)),
                    Stretch = Stretch.UniformToFill
                };

                ImageCanvas.Children.Add(image);

                foreach (var rectangle in Rectangles)
                {
                    var topLeft = rectangle.Points[0];
                    var bottomRight = rectangle.Points[2];

                    var rect = new System.Windows.Shapes.Rectangle
                    {
                        Width = bottomRight.Item1 - topLeft.Item1,
                        Height = bottomRight.Item2 - topLeft.Item2,
                        Stroke = Brushes.Red,
                        StrokeThickness = 2,
                        Margin = new Thickness(topLeft.Item1, topLeft.Item2, 0, 0)
                    };

                    ImageCanvas.Children.Add(rect);
                }
            }
        }

        private IMarkupProject ExecuteLoadProject(bool showDialog = true)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Выберите папку проекта",
                Filter = "Папки|.",
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Выберите папку"
            };

            bool? result = showDialog ? dialog.ShowDialog() : true;
            if (result != true) return null;

            string selectedFolder = Path.GetDirectoryName(dialog.FileName);
            if (selectedFolder == null) return null;

            IProjectConfigLoader configLoader;
            IMarkupProject markupProject;

            try
            {
                configLoader = new ProjectConfigLoader(selectedFolder);
                markupProject = new Models.MarkupProject(configLoader);
            }
            catch (Exception)
            {
                File.WriteAllText(Path.Combine(selectedFolder, "config.cfg"), ProjectConfigLoader.DefaultConfigString());
                return ExecuteLoadProject(false);
            }

            return markupProject;
        }
    }
}
