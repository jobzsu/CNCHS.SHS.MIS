using SHS.StudentPortal.App.Abstractions;

namespace SHS.StudentPortal.App.Implementations;

public class LoadingService : ILoadingService
{
    private bool _isLoading = false;

    public bool IsLoading
    {
        get { return _isLoading; }
        set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                OnLoadingChanged?.Invoke(_isLoading);
            }
        }
    }

    public event Action<bool> OnLoadingChanged;
}
