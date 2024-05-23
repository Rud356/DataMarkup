using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using Prism.Regions;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.ComponentModel;
using App.ProjectSettings.Models;
using App.ProjectSettings.Models.Interfaces;
using App.MarkupProject.Models.Interfaces;
using System.Collections.ObjectModel;
using App.MarkupProject.Models;
using App.ProjectSettings.DTO;
using System.Linq;
using System.Reactive.Linq;


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

        private Models.Polygon? tempPoly = null;

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

        public ICommand CanvasKeyStrokeCommand { get; }

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

        public ObservableCollection<Polygon>? Polygons {
            get => (ObservableCollection<Polygon>) SelectedImage?.Markup.Where(
                markup => markup.GetType() == typeof(Models.Polygon)
            ).OfType<Polygon>().ToObservable();
        }

        public ObservableCollection<Polygon>? Rectangles
        {
            get => (ObservableCollection<Polygon>)SelectedImage?.Markup.Where(
                markup => markup.GetType() == typeof(Models.Rectangle)
            ).OfType<Rectangle>().ToObservable();
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
            OnCanvasLoadedCommand = new DelegateCommand<RoutedEventArgs>(OnCanvasLoaded);
            OnScrollViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(OnScrollViewLoaded);
            MouseWheelCommand = new DelegateCommand<MouseWheelEventArgs>(OnMouseWheel);
            CanvasKeyStrokeCommand = new DelegateCommand<KeyEventArgs>(OnKeyStrokeEvent);
            // Присваиваем переданный Canvas ImageCanvas
            ImageCanvas = imageCanvas;
            if (Project.ConfigLoader.ProjectConfigObj.MarkupClasses.Count() != 0)
                SelectedMarkupClass = Project.ConfigLoader.ProjectConfigObj.MarkupClasses.ElementAt(0);
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
            switch (SelectedTool)
            {
                case MarkupTool.Rectangle:
                {
                    Point position = e.GetPosition((Image)e.Source);
                    int SelectedClassID = Project.ConfigLoader.ProjectConfigObj.MarkupClasses.IndexOf(SelectedMarkupClass);
                    if (SelectedClassID < 0) 
                    {
                        return;
                    }
                    if (_firstPoint == null)
                    {
                        _firstPoint = position;
                    }
                    else
                    {
                        var topLeft = new Tuple<int, int>((int)_firstPoint.Value.X, (int)_firstPoint.Value.Y);
                        var bottomRight = new Tuple<int, int>((int)position.X, (int)position.Y);
                        var rectangle = new Models.Rectangle(
                            ref _project.Labels,
                            new Tuple<int, int, int, int>
                                (topLeft.Item1, topLeft.Item2, bottomRight.Item1, bottomRight.Item2),
                            SelectedClassID
                        );
                        rectangle.AssignedClass = SelectedMarkupClass;
                        SelectedImage.Markup.Add(rectangle);
                        _firstPoint = null; // Сбросить первую точку для следующего прямоугольника
                        UpdateCanvas();
                    }
                    break;
                }

                case MarkupTool.Polygon:
                {
                    if (tempPoly == null)
                    {
                        tempPoly = new Models.Polygon(ref _project.Labels);
                    }

                    Point position = e.GetPosition((Image)e.Source);
                    Tuple<int, int>? previous = null;
                    try
                    {
                        previous = tempPoly.Points.Last();
                    }
                    catch (Exception ex) { } // Silencing error

                    tempPoly.Points.Add(new Tuple<int, int>((int)position.X, (int)position.Y));

                    if (previous is not null)
                    {
                            
                        var point = tempPoly.Points.Last();

                        var line = new System.Windows.Shapes.Line()
                        {
                            Stroke = System.Windows.Media.Brushes.BlueViolet,
                            X1 = previous.Item1,
                            Y1 = previous.Item2,
                            X2 = point.Item1,
                            Y2 = point.Item2
                        };
                        ImageCanvas.Children.Add(line);
                    }

                    break;
                }
            }
        }

        private void OnKeyStrokeEvent(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectedTool == MarkupTool.Polygon && tempPoly is not null && tempPoly.Points.Count() >= 3)
                {
                    tempPoly.AssignedClass = SelectedMarkupClass;
                    SelectedImage.Markup.Add(tempPoly);
                    tempPoly = null;
                    UpdateCanvas();
                }
            }

            if (e.Key == Key.Escape)
            {
                tempPoly = null;
                _firstPoint = null;
                UpdateCanvas();
            }

            if (e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (tempPoly is not null && tempPoly.Points.Count() > 0)
                {
                    // Remove last added poly line
                    tempPoly.Points.RemoveAt(tempPoly.Points.Count()-1);
                    ImageCanvas.Children.RemoveAt(ImageCanvas.Children.Count-1);
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

            if (Project.Images.Count() > 0)
                SelectedImage = Project.Images.ElementAt(0);
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

                if (markup.GetType() == typeof(Models.Rectangle))
                {
                    Models.Rectangle rectangle = (Models.Rectangle)markup;
                    var topLeft = rectangle.Points[0];
                    var bottomRight = rectangle.Points[2];
                    DrawRectangle(ImageCanvas, topLeft, bottomRight);
                }

                if (markup.GetType() == typeof(Models.Polygon))
                {
                    Models.Polygon polygon = (Models.Polygon)markup;
                    DrawPolygon(ImageCanvas, polygon.Points);
                }
            }
        }

        private void DrawRectangle(Canvas canvas, Tuple<int, int> topCorner, Tuple<int, int> bottomCorner)
        {
            var rect = new System.Windows.Shapes.Rectangle
            {
                Width = bottomCorner.Item1 - topCorner.Item1,
                Height = bottomCorner.Item2 - topCorner.Item2,
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                Margin = new Thickness(topCorner.Item1, topCorner.Item2, 0, 0)
            };
            canvas.Children.Add(rect);
        }

        private void DrawPolygon(Canvas canvas, IEnumerable<Tuple<int, int>> points)
        {
            var previous = points.Last();
            foreach (var point in points)
            {
                var line = new System.Windows.Shapes.Line() {
                    Stroke = Brushes.BlueViolet,
                    X1 = previous.Item1, Y1 = previous.Item2, 
                    X2 = point.Item1, Y2 = point.Item2
                };
                previous = point;
                canvas.Children.Add(line);
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

            string? selectedFolder = System.IO.Path.GetDirectoryName(dialog.FileName);
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
                    System.IO.Path.Combine(selectedFolder, "config.cfg"),
                    ProjectConfigLoader.DefaultConfigString()
                );
                return ExecuteLoadProject(false);
            }

            return markupProject;
        }
    }
}
