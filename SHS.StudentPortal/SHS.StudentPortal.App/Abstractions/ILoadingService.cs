namespace SHS.StudentPortal.App.Abstractions;

public interface ILoadingService
{
    bool IsLoading { get; set; }

    event Action<bool> OnLoadingChanged;
}
