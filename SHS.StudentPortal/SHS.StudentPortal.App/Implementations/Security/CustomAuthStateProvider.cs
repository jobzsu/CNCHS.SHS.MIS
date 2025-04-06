using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using SHS.StudentPortal.App.Abstractions.Security;
using System.Security.Claims;

namespace SHS.StudentPortal.App.Implementations.Security;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IUserStateService _userStateService;

    public CustomAuthStateProvider(IUserStateService userStateService)
    {
        _userStateService = userStateService;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_userStateService.GetUser()));
    }

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        _userStateService.SetUser(user);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void ResetUser()
    {
        _userStateService.ResetUser();

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
