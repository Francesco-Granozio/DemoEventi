using CommunityToolkit.Mvvm.ComponentModel;

namespace DemoEventi.UI.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    private bool _isBusy;
    private string? _title;
    private string? _errorMessage;
    private bool _hasError;

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public string? Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            SetProperty(ref _errorMessage, value);
            HasError = !string.IsNullOrEmpty(value);
        }
    }

    public bool HasError
    {
        get => _hasError;
        set => SetProperty(ref _hasError, value);
    }

    protected void SetError(string? message)
    {
        ErrorMessage = message;
    }

    protected void ClearError()
    {
        ErrorMessage = null;
    }
}
