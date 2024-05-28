using App.MarkupProject.Models.Interfaces;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Configuration;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Prism.Commands;

namespace App.MarkupProject.Models;

/// <summary>
/// Points are given in (x, y) format, with ints repserenting pixels poisiton on image.
/// </summary>
public class Polygon : ReactiveObject, IMarkupFigure
{
    private ObservableCollection<Vertex> points;
    private int _classID;
    private bool _isVisible;
    private bool _isSelected;
    private ObservableCollection<string> _labels;

    public ICommand DragDeltaCommand { get; }

    public Polygon(ref ObservableCollection<string> labels)
    {
        points = new();
        AssignedClassID = -1;
        IsVisible = true;
        _labels = labels;
        DragDeltaCommand = new DelegateCommand<DragDeltaEventArgs>(OnDragDelta);
    }

    public Polygon(ref ObservableCollection<string> labels, int classID) : this(ref labels)
    {
        AssignedClassID = classID;
    }

    public Polygon(ref ObservableCollection<string> labels, int classID, bool isVisible) : this(ref labels, classID)
    {
        IsVisible = isVisible;
    }

    private string _label = "Not assigned";

    [Reactive] public string AssignedClass {
        get {
            return _label;
        }
        set
        {
            _classID = _labels.IndexOf(value);
            if (_classID == -1) _label = "Not assigned";
            _label = _labels[_classID];
        }
    }

    [Reactive] public int AssignedClassID { get => _classID; set => _classID = value; }
    [Reactive] public bool IsVisible {
        get => _isVisible;
        set => this.RaiseAndSetIfChanged(ref _isVisible, value);
    }

    [Reactive] public ObservableCollection<Vertex> Points => points;

    public ref ObservableCollection<string> Labels { get => ref _labels; }

    [Reactive]
    public bool IsSelected {
        get => _isSelected;
        set
        {
            this.RaiseAndSetIfChanged(ref _isSelected, value);
            this.RaisePropertyChanged(nameof(this.IsSelected));
        }
    }

    private void OnDragDelta(DragDeltaEventArgs e)
    {
        // Assuming Vertex has properties X and Y
        var vertex = e.Source as Vertex;
        if (vertex != null)
        {
            vertex.X += (int) e.HorizontalChange;
            vertex.Y += (int) e.VerticalChange;
        }
    }
}
