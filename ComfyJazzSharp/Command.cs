using System.ComponentModel;
using System.Windows.Input;

namespace ComfyJazzSharp;

public class Command : ICommand, INotifyPropertyChanged
{
    private readonly Action _action;
    private bool _enabled;

    public Command(Action action)
    {
        _enabled = true;
        _action = action;
    }

    public event EventHandler? CanExecuteChanged;

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsEnabled
    {
        get => _enabled;
        set
        {
            if (_enabled == value)
                return;
            _enabled = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool CanExecute(object? parameter) => IsEnabled;

    public void Execute(object? parameter)
    {
        if (!IsEnabled)
            return;

        _action();
    }
}