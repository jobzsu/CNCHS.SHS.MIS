using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SHS.StudentPortal.App.Abstractions.Security;
using System.Security.Claims;
using System.Text.Json;

namespace SHS.StudentPortal.App.Implementations.Security;

public class UserStateService : IUserStateService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<UserStateService> _logger;

    private const string UserDataKey = "userData";
    private bool _dataLoaded = false;

    public UserStateService(IJSRuntime jsRuntime, 
        ILogger<UserStateService> logger)
    {
        _jsRuntime = jsRuntime;
        LoadUserFromSessionStorage();
        _logger = logger;
    }

    public async Task Initialize()
    {
        if(!_dataLoaded)
        {
            await LoadUserFromSessionStorage();

            _dataLoaded = true;
        }
    }

    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    public ClaimsPrincipal GetUser() => _currentUser;

    public void SetUser(ClaimsPrincipal user)
    {
        _currentUser = user;

        SaveUserToSessionStorage();
    }

    public void ResetUser()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        RemoveUserFromSessionStorage();
    }

    private async Task SaveUserToSessionStorage()
    {
        if (_currentUser.Identity?.IsAuthenticated == true)
        {
            var claimsData = _currentUser.Claims.Select(c => new { c.Type, c.Value }).ToList();
            var jsonData = JsonSerializer.Serialize(claimsData);
            await _jsRuntime.InvokeVoidAsync("blazorSessionStorage.setItem", UserDataKey, jsonData);
        }
        else
        {
            RemoveUserFromSessionStorage();
        }
    }

    private async Task LoadUserFromSessionStorage()
    {
        var jsonData = await _jsRuntime.InvokeAsync<string>("blazorSessionStorage.getItem", UserDataKey);
        if (!string.IsNullOrEmpty(jsonData))
        {
            try
            {
                var claimsData = JsonSerializer.Deserialize<List<ClaimData>>(jsonData);

                if (claimsData != null)
                {
                    var claims = claimsData.Select(c => new Claim(c.Type, c.Value)).ToList();
                    var identity = new ClaimsIdentity(claims, "sessionStorageAuth");
                    _currentUser = new ClaimsPrincipal(identity);
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error Loading User from Session");

                RemoveUserFromSessionStorage();
            }
        }
    }

    private async Task RemoveUserFromSessionStorage()
    {
        await _jsRuntime.InvokeVoidAsync("blazorSessionStorage.removeItem", UserDataKey);
    }

    private class ClaimData
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
