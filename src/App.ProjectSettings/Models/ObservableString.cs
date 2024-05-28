// ObservableString.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class ObservableString : INotifyPropertyChanged
{
    private string _value;
    public string Value
    {
        get { return _value; }
        set
        {
            if (_value != value)
            {
                _value = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public override string ToString()
    {
        return Value;
    }
}
