﻿using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using Prism.Regions;
using Microsoft.Win32;
using App.MarkupProject.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using App.ProjectSettings.Models;
using App.ProjectSettings.Models.Interfaces;
using App.MarkupProject.Models.Interfaces;
using System.Windows.Media.Media3D;

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

        private ScrollViewer _scrollViewer;

        public ScrollViewer MainScrollViewer
        {
            get { return _scrollViewer; }
            set { SetProperty(ref _scrollViewer, value); }
        }

        public double ScrollViewWidth { get => ImageCanvas.ActualWidth - 100; }

        public double ScrollViewHeight { get => ImageCanvas.ActualHeight - 100; }

        public ScaleTransform Scale { get; } = new ScaleTransform();

        public ICommand PolygonToolCommand { get; }
        public ICommand RectangleToolCommand { get; }
        public ICommand DeleteToolCommand { get; }
        public ICommand ChooseFigureToolCommand { get; }
        public ICommand MoveFigureToolCommand { get; }
        public ICommand MovePointsToolCommand { get; }
        public ICommand GoBackCommand { get; set; }
        public ICommand MouseLeftButtonDownCommand { get; set; }
        public ICommand MouseWheelCommand { get; }

        public ICommand OnCanvasLoadedCommand { get; set; }

        public ICommand OnScrollViewLoadedCommand {  get; set; }

        private IMarkupProject _project;

        public IMarkupProject Project { get => _project; }

        private IMarkupImage _selectedImage;

        public IMarkupImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
                UpdateCanvas();
            }
        }

        public string SelectedMarkupClass { get; set; }

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
            OnCanvasLoadedCommand = new DelegateCommand<RoutedEventArgs>(OnCanvasLoaded);
            OnScrollViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(OnScrollViewLoaded);
            MouseWheelCommand = new DelegateCommand<MouseWheelEventArgs>(OnMouseWheel);

            // Присваиваем переданный Canvas ImageCanvas
            ImageCanvas = imageCanvas;
        }

        private void OnMouseWheel(MouseWheelEventArgs e)
        {
            const double SCALE_K = 1.1;
            Point position = e.GetPosition(ImageCanvas);
            Scale.CenterX = position.X;
            Scale.CenterY = position.Y;

            if (e.Delta > 0)
            {
                Scale.ScaleX = Scale.ScaleX * SCALE_K;
                Scale.ScaleY = Scale.ScaleY * SCALE_K;
            }
            else
            {
                Scale.ScaleX = Scale.ScaleX / SCALE_K;
                Scale.ScaleY = Scale.ScaleY / SCALE_K;
            }
            
            e.Handled = true;
        }

        private void MouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (SelectedTool == MarkupTool.Rectangle)
            {
                Point position = e.GetPosition((Image)e.Source);

                if (_firstPoint == null)
                {
                    _firstPoint = position;
                }
                else
                {
                    var topLeft = new Tuple<int, int>((int)_firstPoint.Value.X, (int)_firstPoint.Value.Y);
                    var bottomRight = new Tuple<int, int>((int)position.X, (int)position.Y);
                    var rectangle = new Rectangle(new Tuple<int, int, int, int>(topLeft.Item1, topLeft.Item2, bottomRight.Item1, bottomRight.Item2), 0);

                    SelectedImage.Markup.Add(rectangle);
                    _firstPoint = null; // Сбросить первую точку для следующего прямоугольника
                    UpdateCanvas();
                }
            }
        }

        private void ExecutePolygonTool()
        {
            SelectedTool = MarkupTool.Polygon;
            _firstPoint = null;
            UpdateCanvas();
        }

        private void ExecuteRectangleTool()
        {
            SelectedTool = MarkupTool.Rectangle;
            _firstPoint = null;
            UpdateCanvas();
        }

        private void OnCanvasLoaded(RoutedEventArgs e)
        {
            _imageCanvas = (Canvas) e.Source;
        }

        private void OnScrollViewLoaded(RoutedEventArgs e)
        {
            _scrollViewer = (ScrollViewer)e.Source;
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
            // Clear unused canvas elements
            Image img = (Image)ImageCanvas.Children[0];
            ImageCanvas.Children.Clear();
            ImageCanvas.Children.Add(img);

            foreach (var markup in SelectedImage.Markup)
            {
                if (markup.IsHidden)
                    // Skip rendering
                    continue;

                if (markup is Rectangle)
                {
                    Rectangle rectangle = (Rectangle)markup;
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

        private IMarkupProject? ExecuteLoadProject(bool showDialog = true)
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

            string? selectedFolder = Path.GetDirectoryName(dialog.FileName);
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
                File.WriteAllText(
                    Path.Combine(selectedFolder, "config.cfg"),
                    ProjectConfigLoader.DefaultConfigString()
                );
                return ExecuteLoadProject(false);
            }

            return markupProject;
        }
    }
}
