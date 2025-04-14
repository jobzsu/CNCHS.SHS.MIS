using System.Security.Claims;

namespace SHS.StudentPortal.App.Abstractions.Security;

public interface IUserStateService
{
    ClaimsPrincipal GetUser();

    void SetUser(ClaimsPrincipal user);

    void ResetUser();

    Task Initialize();
}
