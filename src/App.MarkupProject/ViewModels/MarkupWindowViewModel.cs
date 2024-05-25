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
using System.Runtime;
using App.MarkupProject.Tools;
using ReactiveUI;
using Prism.Events;
using App.ProjectSettings.Events;
using DynamicData;


namespace App.MarkupProject.ViewModels;

public enum MarkupTool
{
    Polygon,
    Rectangle,
    Delete,
    ChooseFigure,
    MoveFigure,
    MovePoints
}

internal class MarkupWindowViewModel : BindableBase, INavigationAware
{
    private readonly IEventAggregator _eventAggregator;

    private Vertex? position = null; // Первая точка для создания прямоугольника

    public Vertex? Position
    {
        get => position;
        set
        {
            SetProperty(ref position, value);
        }
    }

    private Models.Polygon? tempPoly = null;

    public Polygon? TempPoly {  get => tempPoly; }

    private MarkupTool _selectedTool;
    public MarkupTool SelectedTool
    {
        get
        {
            return _selectedTool;
        }
        set 
        {
            SetProperty(ref _selectedTool, value);
        }
    }

    private IMarkupFigure? _selectedMarkupDisplay;
    public IMarkupFigure? SelectedMarkupDisplay
    {
        get
        {
            return _selectedMarkupDisplay;
        }
        set
        {
            SetProperty(ref _selectedMarkupDisplay, value);
        }
    }

    public ScaleTransform Scale { get; } = new ScaleTransform();

    public ICommand PolygonToolCommand { get; }
    public ICommand RectangleToolCommand { get; }
    public ICommand DeleteToolCommand { get; }
    public ICommand MoveFigureToolCommand { get; }
    public ICommand MovePointsToolCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand MouseLeftButtonDownCommand { get; }
    public ICommand MouseWheelCommand { get; }
    public ICommand PolygonClickedCommand { get; }
    public ICommand CanvasKeyStrokeCommand { get; }

    public ICommand SelectionKeyboardCommand { get; }

    public ICommand ToSettings { get; }

    private IMarkupProject _project;

    public IMarkupProject Project => _project;

    private IMarkupImage _selectedImage;

    public IMarkupImage SelectedImage
    {
        get => _selectedImage;
        set
        {
            _selectedImage = value;
            Scale.ScaleX = 1;
            Scale.ScaleY = 1;
            OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
        }
    }


    private string _selectedMarkup;
    public string SelectedMarkupClass
    {
        get => _selectedMarkup;
        set
        {
            SetProperty(ref _selectedMarkup, value);
        }
    }

    public MarkupWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        MouseLeftButtonDownCommand = new DelegateCommand<MouseButtonEventArgs>(MouseLeftButtonDown);
        GoBackCommand = new DelegateCommand(() =>
            {
                regionManager.RequestNavigate("MainRegion", "MainView");
            }
        );
        if (_project is null)
            _project = ExecuteLoadProject();

        if (_project is null) {
            regionManager.RequestNavigate("MainRegion", "MainView");
            return;
        }

        ToSettings = new DelegateCommand(
            () =>
            {
                regionManager.RequestNavigate("MainRegion", "ProjectSettingsView");
                _eventAggregator.GetEvent<ConfigSharingEvent>().Publish(_project.ConfigLoader);
            }
        );

        PolygonToolCommand = new DelegateCommand(ExecutePolygonTool);
        RectangleToolCommand = new DelegateCommand(ExecuteRectangleTool);
        DeleteToolCommand = new DelegateCommand(ExecuteDeleteTool);
        MoveFigureToolCommand = new DelegateCommand(ExecuteMoveFigureTool);
        MovePointsToolCommand = new DelegateCommand(ExecuteMovePointsTool);
        MouseWheelCommand = new DelegateCommand<MouseWheelEventArgs>(OnMouseWheel);
        CanvasKeyStrokeCommand = new DelegateCommand<KeyEventArgs>(OnKeyStrokeEvent);
        SelectionKeyboardCommand = new DelegateCommand<KeyEventArgs>(SelectionKeyboard);
        PolygonClickedCommand = new DelegateCommand<Polygon>(PolygonClicked);
    }

    private void OnMouseWheel(MouseWheelEventArgs e)
    {
        const double SCALE_K = 1.1;
        Point position = e.GetPosition((UIElement) e.Source);
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
                if (e.Source is not Canvas) break;
                Point pos = e.GetPosition((Canvas) e.Source);
                int SelectedClassID = Project.ConfigLoader.ProjectConfigObj.MarkupClasses.IndexOf(SelectedMarkupClass);
                if (SelectedClassID < 0) 
                {
                    return;
                }
                if (position == null)
                {
                    position = new Vertex((int)pos.X, (int)pos.Y);
                }
                else
                {
                    var topLeft = new Tuple<int, int>((int)pos.X, (int)pos.Y);
                    var bottomRight = new Tuple<int, int>((int)position.X, (int)position.Y);
                    var rectangle = new Models.Rectangle(
                        ref _project.Labels,
                        new Tuple<int, int, int, int>
                            (topLeft.Item1, topLeft.Item2, bottomRight.Item1, bottomRight.Item2),
                        SelectedClassID
                    );
                    rectangle.AssignedClass = SelectedMarkupClass;
                    SelectedImage.Markup.Add(rectangle);
                    position = null; // Сбросить первую точку для следующего прямоугольника
                }
                break;
            }

            case MarkupTool.Polygon:
            {
                if (e.Source is not Canvas)
                    break;
                if (tempPoly == null)
                {
                    tempPoly = new Models.Polygon(ref _project.Labels);
                    SelectedImage.Markup.Add(tempPoly);
                }
                var i = SelectedImage.Markup.IndexOf(tempPoly);

                tempPoly = (Polygon) SelectedImage.Markup[i];
                SelectedImage.Markup.RemoveAt(i);

                Point position = e.GetPosition((Canvas) e.Source);
                tempPoly.Points.Add(new Vertex((int)position.X, (int)position.Y));
                SelectedImage.Markup.Add(tempPoly);
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
                SelectedImage.Markup.Remove(tempPoly);
                tempPoly.AssignedClass = SelectedMarkupClass;
                SelectedImage.Markup.Add(tempPoly);
                tempPoly = null;
            }
            else
            {
                tempPoly = null;
            }
        }

        if (e.Key == Key.Escape)
        {
            if (tempPoly is not null)
                SelectedImage.Markup.Remove(tempPoly);

            tempPoly = null;
            position = null;
        }

        if (e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
        {
            if (tempPoly is not null && tempPoly.Points.Count() > 0)
            {
                if (tempPoly.Points.Count() - 1 == 0)
                {
                    SelectedImage.Markup.Remove(tempPoly);
                    tempPoly = null;
                    return;
                }

                // Remove last added poly line
                var i = SelectedImage.Markup.IndexOf(tempPoly);

                tempPoly = (Polygon)SelectedImage.Markup[i];
                SelectedImage.Markup.RemoveAt(i);
                tempPoly.Points.RemoveAt(tempPoly.Points.Count() - 1);
                SelectedImage.Markup.Add(tempPoly);
            }
        }

        if (e.Key == Key.Delete && SelectedMarkupDisplay is not null)
        {
            SelectedImage.Markup.Remove(SelectedMarkupDisplay);
            SelectedMarkupDisplay = null;
        }

        if (e.Key == Key.H && SelectedMarkupDisplay is not null)
        {
            SelectedMarkupDisplay.IsVisible = !SelectedMarkupDisplay.IsVisible;
        }
    }

    private void SelectionKeyboard(KeyEventArgs e)
    {
        if (e.Key == Key.Delete && SelectedMarkupDisplay is not null)
        {
            SelectedImage.Markup.Remove(SelectedMarkupDisplay);
        }

        if (e.Key == Key.H && SelectedMarkupDisplay is not null)
        {
            SelectedMarkupDisplay.IsVisible = !SelectedMarkupDisplay.IsVisible;
        }
    }

    private void PolygonClicked(Polygon poly)
    {
        SetPolygonSelection(poly); 
    }

    private void ExecutePolygonTool()
    {
        SelectedTool = MarkupTool.Polygon;
        position = null;
    }

    private void ExecuteRectangleTool()
    {
        SelectedTool = MarkupTool.Rectangle;
        position = null;
    }

    private void ExecuteDeleteTool()
    {
        SelectedTool = MarkupTool.Delete;
    }

    private void ExecuteMoveFigureTool()
    {
        SelectedTool = MarkupTool.MoveFigure;
    }

    private void ExecuteMovePointsTool()
    {
        SelectedTool = MarkupTool.MovePoints;
    }

    private void SetPolygonSelection(Polygon poly)
    {
        if (SelectedMarkupDisplay != null)
        {
            // Find the selected polygon in the list
            var selected = SelectedImage.Markup.FirstOrDefault(p => p == SelectedMarkupDisplay);
            if (selected != null)
            {
                selected.IsSelected = false;
                SelectedImage.Markup.Remove(selected);
                SelectedImage.Markup.Add(selected);
            }
            SelectedMarkupDisplay = null;
        }


        poly.IsSelected = true;
        var i = SelectedImage.Markup.IndexOf(poly);
        if (i >= 0)
        {
            SelectedImage.Markup.RemoveAt(i);
            SelectedImage.Markup.Add(poly);
            SelectedMarkupDisplay = SelectedImage.Markup.Last();
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

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        // Force refresh
        Project.ConfigLoader.ProjectConfigObj.MarkupClasses.Add("");
        Project.ConfigLoader.ProjectConfigObj.MarkupClasses.RemoveAt(Project.ConfigLoader.ProjectConfigObj.MarkupClasses.Count()-1);

        if (Project.ConfigLoader.ProjectConfigObj.MarkupClasses.Count() != 0)
            SelectedMarkupClass = Project.ConfigLoader.ProjectConfigObj.MarkupClasses.ElementAt(0);
    }

    private bool IsNavTargert = true;

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return IsNavTargert;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        if (navigationContext.Uri.ToString() == "MainView")
        {
            IsNavTargert = false;
        }
    }
}
